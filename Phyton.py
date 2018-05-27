import requests
import json
import random
import pprint
import time as t
import os
import RPi.GPIO as GPIO
import datetime

GPIO.setmode(GPIO.BCM)
GPIO.setup(18, GPIO.IN)

suDoldur = False
suBosalt = False
headers = {'Content-type': 'application/json'}
url = 'https://iothook.com/api/latest/datas/'
while(1):
    if GPIO.input(18) == True:
        suDoldur = True
    if GPIO.input(18) == False:
        if(suDoldur):
            suBosalt = True
            suDoldur = False
    if(suBosalt):
        time = datetime.datetime.now()
        date = datetime.datetime.strftime(time, '%d.%m.%Y')
        hour = datetime.datetime.strftime(time, '%X')
        data={
        'api_key': 'd190a3db-5e27-11f76d19b3aae6428d', # demo hesap api_key
        'value_1': '1-'+str(date)+str(hour),   
        }
        data_json = json.dumps(data)
        response = requests.post(url, data=data_json, headers=headers)
        pprint.pprint(response.json())
        print('su icildi')
        suBosalt = False
        t.sleep(3)


