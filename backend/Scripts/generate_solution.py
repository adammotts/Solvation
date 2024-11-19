import requests

url = "http://localhost:5256/game-state"

payload = {
    "PlayerSumValue": 21,
    "PlayerValueType": "Blackjack",
    "PlayerStateType": "Terminal",
    "DealerFaceUpValue": 10,
    "DealerValueType": "Hard",
    "DealerStateType": "Active",
    "Actions": {
        "Hit": 0.65,
        "Stand": 0.85,
        "Double": 1.25,
        "Split": 0.95,
    }
}

headers = {
    "Content-Type": "application/json"
}

response = requests.post(url, json=payload, headers=headers)

if response.status_code == 200:
    print("Success!")
    print("Response Data:", response.json())
else:
    print(f"Failed with status code {response.status_code}")
    print("Error:", response.text)
