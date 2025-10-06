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

---

# Projekteinführung
Dieses Projekt ist ein einfaches C#-Programm, das Ihnen eine Aktivität von einer Web-API namens Bored-Activity (https://bored-api.appbrewery.com/) bereitstellt. Anschließend speichert es die neu angeforderte Aktivität über SQLite und zeigt Ihnen alle Ihre zuvor gespeicherten Aktivitäten an.

Viel Spaß (nicht) bei der Langeweile

# Umfang
Ziel ist es, ein C#-Projekt zu erstellen, das auf eine API zugreift und etwas in einer SQLite-Datenbank speichert

# Zeitaufwand
Es dauerte etwa 6 Stunden, dieses Projekt zu programmieren und funktionsfähig zu machen, einschließlich 2 Stunden Recherche zu APIs und Erlernen der Datenbanknutzung in C#.

# Verfahren
- Die ausgewählte API wurde ausgewählt, weil sie sehr einfach ist und in noch einfachere Verwendungszwecke unterteilt werden kann, was perfekt für den Projektumfang war. Sie können das Rauschen der API-Antwort reduzieren, indem Sie einfach alle unerwünschten Aktivitäten ausschließen. Attribute, um nur die wichtigen Attribute beizubehalten, die Sie benötigen. In meinem Fall waren das:
    - Aktivität.Aktivität (der grundlegende Name/die grundlegende Beschreibung der angegebenen Aktivität)
    - Aktivität.Typ (ziemlich selbsterklärend)
    - Aktivität.Teilnehmer (Anzahl der Teilnehmer)

---