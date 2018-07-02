# cosential-client-csharp
Description: A Client library written in C# for Compass (Cosential's general purpose RESTful API).

It is highly recommended to test the library on the UAT Environment before moving to PROD.

Please refer to the 'Contexts' folder under the namespace 'CompassApiClient' for CRUD operations on Objects & SubObjects.

Steps to run the sample tests:
- Download zip and extract.
- Open the solution file (.sln) in Microsoft VS.
- Under the namespace 'CompassApiClientTests' look for the file 'Credentials.cs.example' and rename it to 'Credentials.cs'.
- Open the file and enter the credentials for the firm. Save.
- Change the test method as per the requirement and run the sample test.

Note (for PUT operations): *** The user should make two calls to the Compass:
- First Call: GET the existing record at Cosential. Modify and set the properties of the Object as desired.
- Second Call: Send (PUT) the updated record to Cosential.
