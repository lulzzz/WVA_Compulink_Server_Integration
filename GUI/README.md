


If you are having issues installing the 'WVA Compulink Server Interation' Windows service see the help below.
---------------------------------------------------------------------------------------------------------------------------

1 ) Open up command prompt as administrator (you will need elevated privileges to install Windows services)

2 ) Open up Windows Services menu to monitor the status of our service properly

3 ) Run the command: sc create WVA_Compulink_Server_Integration binPath="C:\Users\userName\AppData\Local\WVA_Compulink_Server_Integration\app-1.1.0\Server\WVA_Compulink_Server_Integration.exe"
	- NOTE: The app will be located in the '\AppData\Local\' of whichever user installed the application on your server

4 ) After running the above command, you should see something like '[SC] CreateService SUCCESS' in the command prompt output
	and the 'WVA_Compulink_Server_Integration' service will appear near the bottom of your Windows services menu.

5 ) Once the service is installed, run the command: net start WVA_Compulink_Server_Integration
	- You should see the 'WVA_Compulink_Server_Integration' service marked with the status 'running' in the Services menu 
	
6 ) If the service is not running after several attempts, please contact the WVA Support team at 800-747-9000 x8191


