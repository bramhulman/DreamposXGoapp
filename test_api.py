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

# Get Member (Archen's UID)
member_url = 'https://api.goapp.co.id/channel/v1/member/member/155999749384776/'
s_resp = requests.get(member_url, headers=headers, verify=False)
print("Member Response:", s_resp.status_code, json.dumps(s_resp.json(), indent=2))
