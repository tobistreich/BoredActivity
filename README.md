# Project Introduction
This Project is a simple C# program that gives you an activity from a Web-API called Bored-Activity (https://bored-api.appbrewery.com/) it then saves the newly requested activity via SQLite and shows you all your previously saved activities.

Have fun (not) being bored

# Scope 
Goal is to create a C# project that accesses an API and saves something in an SQLite DB

# Time Expedenture
This project took about 6 hours to program and make functional including 2 hours of research on APIs and learning about Database use in C#

# Procedure
- The chosen API was picked beacuse it is very simple and can be broken down into even simpler use, which was perfect for the project scope. You can reduce the noise of the API Response by simply excluding all unwanted activity.Attributes, to keep only the important attributes you need. In my case these were:
  - activity.Activity (the basic name/description of the given activity)
  - activity.Type (pretty self explanatory)
  - activity.Participants (number of participants)
