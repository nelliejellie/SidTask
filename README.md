# Assignment 1 & 2
in this repo contains the implementation for the task 3 and 4 that was given which are

- building an mvc webb app using identity for authentication and provisioning roles to users that determine which pages they are authorised to use
- data base query optimization using entitframework linq queries and raw sql queries

  # HOW TO RUN THE PROJECT
- install visual studio and clone this repo to your repository
- click on the soltion file( the file that ends with a .sln extension)
- once the project opens and you have a good internet connection the dependencies should be installed automatically
- another option for installing depencies is to go to the csproj file and install one after the other
- Navigate to the appsetting.json and make sure you put the right credentials for your sqlserver username and password

once the processes above has been done correctly then start the application by clicking on the green play button on visual studio,
all migrations should be applied and the program should run very fine.

# HOW TO TEST TASK ONE
once the program starts running you should be redirected to the login page
an admin credential has already been seeded to the db which means you can automatically login as an admin once the project starts
the username/email = ***admin@example.com***
the password = ***Admin@123***
once you put in these credentials you should be redirected to the admin dashboard

to login as a user just navigate to the register and create a new account and once you login then you should be navigated to the user dashboard

# HOW TO CHECK TASK TWO
task two implementaions are in the repository folder so i created optimised queries for both linq and raw sql and seperated them in two classes named
***CustomerRepository***
***CustomerRawSqlRepository***
each of these classes contains different ways to query the db but achieving the same result
