import asyncio
import os
from asyncio import StreamWriter
from collections.abc import Callable
from typing import TextIO, Any

import websockets
import json
import datetime
from pathlib import Path

def safe_dispatch(func: Callable) -> Callable:
    async def wrapper(self, websocket: Any, *args, **kwargs):
        try:
            return await func(self, websocket, *args, **kwargs)

        except websockets.exceptions.ConnectionClosed:
            print("[INFO] Client closed connection.")
        except Exception as e:
            print(f"[ERROR] Logic error: {e}")
        finally:
            if websocket in self._connected_clients:
                self._connected_clients.remove(websocket)
                print("[DEBUG] Safety cleanup: Client removed.")
    return wrapper



class Server:
    _connected_clients: Any
    _log_filepath: Path

    def __init__(self):
        self._log_filepath = Path("logs.json")
        self._init_file()
        self._connected_clients = set()

    def _init_file(self):
        if not self._log_filepath.exists() or self._log_filepath.stat().st_size == 0:
            self._log_filepath.parent.mkdir(parents=True, exist_ok=True)
            self.save_logs([])

    def load_logs(self):
        with open(self._log_filepath, "r") as f:
            return json.load(f)


    def save_logs(self, logs):
        with open(self._log_filepath, "w") as f:
            json.dump(logs, f, indent=2)


    async def run(self):
        async with websockets.serve(self.dispatch, "localhost", 8765):
            print("Server started on ws://localhost:8765")
            await asyncio.Future()  # run forever


    async def send_logs(self, websocket):
        logs = self.load_logs()
        await websocket.send(f"logs:{json.dumps(logs)}")
        print("logs_send")

    async def receive_mess(self, websocket, mess: str):
        content = json.loads(mess)
        user = content.get("User", "anonymous")
        msg_text = content.get("Content", "")

        entry = {
            "User": user,
            "Content": msg_text,
        }

        logs = self.load_logs()
        logs.append(entry)
        self.save_logs(logs)
        print("Broadcasting to", len(self._connected_clients), "clients")
        broadcast = f"mess:{json.dumps(entry)}"
        await asyncio.gather(*[
            client.send(broadcast)
            for client in self._connected_clients
        ])

    @safe_dispatch
    async def dispatch(self, websocket):
        self._connected_clients.add(websocket)
        async for request in websocket:
            match request:
                case "get_logs":
                    await self.send_logs(websocket)
                case msg if msg.startswith("mess:"):
                    await self.receive_mess(websocket, request[5:])
                case _:
                    await websocket.send("Invalid request")
                    raise ValueError("Invalid request")


def main():
    server = Server()
    asyncio.run(server.run())

if __name__ == "__main__":
    main()