# step-by-step

This is a series of notes to provide step by step instructions on how to set up a project like this from scratch

- Followed Mark Heath's post on setting up a new function with Docker Support
  - https://markheath.net/post/azure-functions-docker
  - Creating a Dockerfile section
- Added a new function in Visual Studio, using the projects right-click -> Add "New Azure Function..." option
- Decided on some basic fake data for people, adding the Models/Person.cs and Data.cs files
- Established GET and POST endpoints with several branching routes, which will provide scenarios for testing in Postman
- Added in Microsoft.Azure.WebJobs.Extensions.OpenApi to decorate the two functions with OpenApi information to give a little documentation and quick import to postman
- Once importated into Postman - exported the collection as a JSON file and added it to this project. This way anyone can import the collection into postman and see the tests, run them and propose new changes by exporting the collection back to source control.