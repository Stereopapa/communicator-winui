import asyncio
import websockets
import json
import datetime
from pathlib import Path

connected_clients = set()
LOG_FILE = "logs.json"

def load_logs():
    if Path(LOG_FILE).exists():
        with open(LOG_FILE, "r") as f:
            return json.load(f)
    return []

def save_logs(logs):
    with open(LOG_FILE, "w") as f:
        json.dump(logs, f, indent=2)

async def handler(websocket):
    connected_clients.add(websocket)
    try:
        async for message in websocket:
            if message == "get_logs":
                logs = load_logs()
                await websocket.send(f"logs:{json.dumps(logs)}")
                print("logs_send")
            elif message.startswith("mess:"):
                text = message[5:]

                content = json.loads(text)  # ✅ Parse to dict
                user = content.get("User", "anonymous")
                msg_text = content.get("Content", "")

                entry = {
                    "User": user,
                    "Content": msg_text,
                }

                logs = load_logs()
                logs.append(entry)
                save_logs(logs)
                print("Broadcasting to", len(connected_clients), "clients")
                broadcast = f"mess:{json.dumps(entry)}"
                await asyncio.gather(*[
                    client.send(broadcast)
                    for client in connected_clients
                ])
            else:
                print("inwalid Command")
                await websocket.send("Invalid command.")
    except websockets.exceptions.ConnectionClosed:
        pass
    finally:
        connected_clients.remove(websocket)

async def main():
    async with websockets.serve(handler, "localhost", 8765):
        print("Server started on ws://localhost:8765")
        await asyncio.Future()  # run forever

if __name__ == "__main__":
    asyncio.run(main())