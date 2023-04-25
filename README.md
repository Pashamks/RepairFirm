ReadMe file for ASP.NET MVC project - Apartment Repair Management System
This is an ASP.NET MVC project for managing and organizing apartment repairs for a repair company. The application is designed to provide a platform where users can log in, filter repairs by all parameters, view diagrams and download data in PDF format. The project uses a SQL Server database to store and manage data related to repairs, clients, employees, etc.

Project Setup
To use this application, you need to have Visual Studio 2019 and SQL Server Management Studio (SSMS) installed on your machine. You can follow the steps below to set up the project:

Clone the repository to your local machine
Open the project in Visual Studio 2019
Open the App_Data\ApartmentRepairDB.mdf database file in SSMS and execute the ApartmentRepairDB.sql script to create the database schema and tables
Change the database connection string in Web.config to point to your local SQL Server instance
Build and run the project using Visual Studio 2019
Project Structure
The project structure is divided into several folders:

Controllers: Contains the controllers that handle incoming requests and define actions to be taken based on those requests.
Models: Contains the model classes that represent the entities in the database and the data transfer objects (DTOs) used to transfer data between the controllers and views.
Views: Contains the Razor views that display the data to the users.
Scripts: Contains JavaScript files used to enhance the user interface.
Content: Contains CSS files used to style the views.
Features
The following are the features implemented in this project:

User authentication: Users can create accounts and log in to the system.
Repair management: Users can create, edit, delete and view repair requests.
Client management: Users can add, edit, delete and view clients' details.
Employee management: Users can add, edit, delete and view employee details.
PDF Export: Users can download repair and client data in PDF format.
Data filtering: Users can filter repairs by all parameters.
Charting: Users can view charts of repairs based on various parameters.
Technologies Used
ASP.NET MVC
C#
Entity Framework
SQL Server
HTML
CSS
JavaScript
Bootstrap
Conclusion
This project is a robust and user-friendly application for managing and organizing apartment repairs. It offers a secure platform for users to log in, filter repairs by all parameters, view diagrams, and download data in PDF format. It is a great starting point for anyone looking to build a similar application in the future.
