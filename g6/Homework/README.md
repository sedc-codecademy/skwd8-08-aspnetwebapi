# Todo application

### Requirements
We want to create a TODO application, we have the client that will consume the data we only need service that will process that data. 

### Bonus
Research and collaborate as a team, every question that you will have first consult with the team and if you need more clarification create a text file that will contain most of the question and suggestions and send that file to Tosho or Trajan to clarify. 
Also you will be given the freedom to create your own tasks so that every project will be unique on its own way.
Every team member should create his own MVP application. 
As a clarification by the term Team i mean the whole group should collaborate for creating the task and question regarding the application.

### Tips and tricks
Be creative and overengineer stuff

### Q&A
Q: What is MVP?

A: [MVP is]

Q: Do we need to create our own database?

A: Yes, you need to create the database on your own. The approach that you want to take is all up to you. Meaning that we do not mind if is code first or database first.

Suggestion for models:
User, Task
* Task is already defined class in System.Threding.Tasks so use Todo as the name of the class.

The models that you suggested are good but we need for every Task to have subtasks. 

example: 

Task: Create Todo Application
Subtasks:
* Reaserch and consult with client - in progress
* Create the application - in progress
* Add swagger - not started
* ....

Q: Do we need services or we could use only controllers?

A: The approach or the architecture that you are using will determent if you need services or not.

ex. if you are not using n-tier architecture and the whole application including connection with db and business logic is set in the application then you could skip the services.

Q: What data the client will expect to get?

A: Depending on end point

ex. if the client wants to get all Todos the json should look like:
```json
{
    "todos":[
        {
            "title": "Create web api",
            "status": "in-progress",
            "dateCreated": "10.10.2020",
            "dateFinished": null,
            .... 
            "subtasks": [
                {
                    "title": "Create connection with database",
                    "status": "completed",
                    "dateCreated": "10.10.2020",
                    "dateFinished": "10.10.2020",
                    ...
                }
            ]
        }
    ],
    "todosCount": ...,
}
```
As for the user end points we only need the basic info for the User.

Q: Do we need register/login?

A: The user should be able to register or login in the application. Maybe in near future we will add some authentication so the api will be secured and only registered user can be able to get the data.



[MVP is]: https://teamairship.com/what-is-mvp-in-software-development/
