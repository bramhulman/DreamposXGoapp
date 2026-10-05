import requests
import json
import urllib3
urllib3.disable_warnings()

auth_url = 'https://account.goapp.co.id/auth/token-auth/'
auth_data = {
    'username': '138350315235400',
    'password': 'cbf5b1c921b36bfb06ca988c6d98f608482c40f1'
}
resp = requests.post(auth_url, json=auth_data, verify=False)
token = resp.json().get('token')
headers = {'Authorization': f'Token {token}'}

# Let's try getting stores
store_url = 'https://api.goapp.co.id/channel/v1/store/'
s_resp = requests.get(store_url, headers=headers, verify=False)
print("Stores:", s_resp.status_code, s_resp.text)
