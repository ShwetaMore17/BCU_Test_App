# BCU.Contoso.Timetable
The Contoso timetable application is a simple set of web pages intended to allow students to view their timetable and events from inside a mobile app.
Details about the project are documented in the [project wiki](https://dev.azure.com/BCUVSTS/Tester%20Assessment/_wiki/wikis/Tester-Assessment.wiki/8/Tester-Technical-Assessment).

## Prerequisites
The solution has been built using .net 9.0 and uses aspire for local development. You will need to install the following software:
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/community/) or later
  - Install the following workloads:
    - ASP.NET and web development
    - Azure development
    - Azure development tools
  - Ensure you also have the following components:
    - .NET Aspire SDK
    - .NET SDK
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
  - This is required to host the database without needing to explicitly install SQL Server locally.

## Running the Application
This solution uses Aspire to orchestrate the local development environment. To run the application, follow these steps:
1. Clone the repository to your local machine.
1. Run docker desktop.
1. Open the solution in Visual Studio 2022.
1. Set BCU.Contoso.Timetable.AppHost as the startup project.
1. Run the application using F5 or Ctrl+F5.

NOTE:: When running the application for the first time, it may take a few minutes to start up as the database is seeded with data. You can check the logs in the output window to see when the application is ready.
If you interrupt the seeding process, you need to delete both the database container and volume and re-run the application to start the seeding process again.

## Local Development
When running the application locally you will be shown the Aspire dashboard. This is a web page that allows you to see the status of the containers and volumes that are running locally. You can also use this page to stop and start the containers and volumes as needed.
Microsoft has extensive documentation on how to use Aspire. You can find this documentation [here](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard/explore#resources-page).