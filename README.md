# Workshop: Gen AI Tool Developing a Web Application

## Purpose of the Application

**TBD** is a web application designed to help businesses manage their customer relationships and financial transactions efficiently.

### Full Application Features
- **User Authentication**: Multiple users can sign in to the application.
- **Customer Management**: Users can track their customers and manage customer information.
- **Project and Work Tracking**: Users can track work performed on individual projects or for specific customers.
- **Invoicing**: Users can issue invoices to their customers.
- **Bank Account Statements**: Users can enter account statements issued by the bank.
- **Invoice Tracking**: Users can track paid and unpaid invoices by pairing them with account statements.

### Development Process

The entire application is developed using generative AI. The purpose of this application is to learn and evaluate generative AI tools for writing software.

The work is divided into a series of lessons:
- In each lesson, we develop one feature at some state of the application.
- The goal of each lesson is to apply the generative AI tools to develop the requested feature.

By following this structured approach, we aim to explore the capabilities and limitations of generative AI in software development.

*Note: This README has also been generated with the help of a generative AI tool.*

## Before the Workshop Begins

To start working with this workshop, you should create your own copy of the repository on GitHub and clone it to your local machine. This ensures you have access to *all branches* used in the lessons and can make changes independently.

### Step 1: Fork the Repository Using the GitHub UI

1. Navigate to the original workshop repository on GitHub.
2. Click the **Fork** button in the upper right corner.
3. Unselect the "Copy the main branch only" option
4. Select your GitHub account as the destination for the fork.

This creates a copy of the repository in your account, including all branches and tags.

### Step 2: Clone Your Fork Locally

1. Go to your forked repository on GitHub.
2. Click the **Code** button and copy the repository URL (HTTPS or SSH).
3. Open a terminal on your computer and run:

   ```
   git clone <your-fork-url>
   ```

4. Change into the cloned directory:

   ```
   cd <repo-name>
   ```

### Step 3: Access All Branches

By default, only the default branch is checked out. To see all branches:

1. Fetch all remote branches:

   ```
   git fetch --all
   ```

2. List all branches (local and remote):

   ```
   git branch -a
   ```

3. To work on a specific lesson branch, check it out:

   ```
   git checkout lesson-05-domain-modeling
   ```

Repeat this for any lesson branch you want to work on.

**Tip:**  
If you want to keep your fork up to date with the original repository, you can add the original as an upstream remote:

```
git remote add upstream <original-repo-url>
git fetch upstream
```

You can then merge or rebase changes from the original repository as needed.

## Running the Demo Application

Follow these steps to run the demo application:

### 1. Set the Connection String

Both the **Web** and **Cli** projects require a connection string to connect to the database.

- Open the `appsettings.Development.json` file in both the `Web` and `Cli` project directories.
- Find the `ConnectionStrings` section.
- Set the connection string to match your environment. For example, for a SQL Server LocalDb instance:

  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=auth;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
  ```

> **Note:** Make sure both projects use the same connection string.

### 2. Initialize the Database

Before running the web application, you need to set up the database schema and create test users.

- Open a terminal and navigate to the `Cli` project directory.
- Run the following command to reset and initialize the database:

  ```
  dotnet run -- resetdb
  ```

This will drop and recreate the database, set up the schema, and create test users (`owner/owner`, `user1/user1`, etc.).

### 3. Run the Web Application

- Open a new terminal and navigate to the `Web` project directory.
- Start the web application:

  ```
  dotnet run
  ```

- Open your browser and go to the URL shown in the terminal output (usually `https://localhost:5001` or similar).

- Log in using the test user credentials user1/user1.

You can now explore the application's features.

## Working With Lesson Branches

Each lesson in the workshop has dedicated branches. You can list all the lessons using the command:

```
git branch
```

These are the branches every lesson has:
- `lesson-NN-topic` - the main branch on which the lesson is developed; initially corresponds to the state of the `main` branch at the time when the lesson starts. You can make changes to this branch as you develop a solution to the lesson.
- `lesson-NN-completed` - the state of the code with the official solution applied; this branch is kept separate from the *regular* branch for the lesson, so that each attendee can make own changes to the code and then compare it to the officially suggested solution (which, to be noted, might not be better than yours in any respect).

There is a tag defined, in the format `lesson-NN-start`, which marks the initial state of the lesson's branch. You can always checkout this state using the command:

```
git checkout lesson-NN-start
```

This will bring your Git repository into a detached HEAD state. If checking out by tag, you can either create a new branch to commit any changes, or stash them for later use.

## Lesson 01: Prompt Engineering Fundamentals

This lesson is implemented on the branch `lesson-01-prompt-fundamentals`.

*To begin* solving the lesson, switch to the tag `lesson-01-start`.

```
git checkout lesson-01-start
```

*Be aware* that this action will leave your repo in a detached HEAD state.

*This lesson* is teaching the fundamental principles of constructing prompts for the generative AI tools. The techniques you will learn include:
- Defining the context, verifications, and the output format
- Specifying the requests
- Instructing the tool to prepare the plan of action

*Your task* is to make changes to the UI.
- Start from the existing `Index.cshtml`
- Use an AI tool to ensure that the page, with all its elements, is rendered using a dark theme.

## Lesson 02: Scaffolding with Gen AI

This lesson is implemented on the branch `lesson-02-scaffolding`.

*To begin* solving the lesson, switch to the tag `lesson-02-start`.

```
git checkout lesson-02-start
```

*Be aware* that this action will leave your repo in a detached HEAD state.

*This lesson* is teaching the virtue of using AI to integrate [Microsoft Identity library](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity) into the ASP.NET Core application. There are many sensitive steps to make in order to scaffold the use of Identity. These tasks can be performed effectively using AI tools, so to replace a human in implementing tedious and brittle steps.

*The request* in this lesson is to implement authentication (login/logout features only), where users are managed in the local database.

*Your task* is to make changes to the application:
- Add necessary packages to the project, to support everything required by Microsoft Identity
- Scaffold the database schema to support Identity
- Make changes to the `_Layout.cshtml` to support login/logout features
- Implement Login/Logout pages that utilize Identity to manage users

## Lesson 03: Exploring a Library

This lesson is implemented on the branch `lesson-03-exploring-library`.

*To begin* solving the lesson, switch to the tag `lesson-03-start`.

```
git checkout lesson-03-start
```

*Be aware* that this action will leave your repo in a detached HEAD state.

*This lesson* is teaching the virtue of using AI to experiment with a library you are not comfortable with. We shall integrate the application with [Spectre.Console](https://spectreconsole.net).

This library brings many specifics we must master, including:
- Registering commands,
- Dependency injection,
- Formatting output, etc.

AI tools are a viable solution to getting acquainted with an unknown library code and still write sound, correct, and maintainable code.

*The request* in this lesson is to implement a side console application intended for use during the development of the main website.

*At the beginning* of the lesson, observe that the authentication has been moved into a separate project, namded `Authentication`. That simplifies the `Web` project, but complicates recreating the database - a frequent operation in development. Dropping and creating the database afresh requires these complicated steps:

```
cd ./Web
dotnet ef database drop --project ../Authentication --context ApplicationDbContext
dotnet ef database update --project ../Authentication --context ApplicationDbContext
```

*Your task* is to develop a simple development tool that automates this task.

Implement these features in the console application:
- Add necessary packages to use `Spectre.Console` in the project
- Scaffold the command, including dependency injection
- Implement a command which resets the database by dropping and updating it again

## Lesson 04: Developing a Feature Plan

This lesson is implemented on the branch `lesson-04-feature-planning`.

*To begin* solving the lesson, switch to the tag `lesson-04-start`.

```
git checkout lesson-04-start
```

*Be aware* that this action will leave your repo in a detached HEAD state.

*This lesson* is teaching how you can utilize an AI tool to develop a step-by-step plan for a new feature before implementing each of the steps.

*The request* is to add a feature to both the main Web application, and to the accompanying CLI tool, to create sample users that can be used in the development.

*Your task* is to add these features:
- Ensure that roles `Owner` and `User` exist in the Identity schema (update the `Authentication` project).
- Ensure that the `resetdb` command in the CLI tool creates these test users (username/password): owner/owner, user1/user1, user2/user2, user3/user3.

Please keep in mind that the passwords of these users do not satisfy the common password validation rules. These users should be a subject to relaxed validation rules on creation.

*To verify* that the task is complete, use the CLI tool:

```
# Run in the Cli directory
dotnet run -- resetdb
```

After executing the command, ensure that the `auth` database schema contains the roles (`Owner`, `User`) and users (`owner`, `user1`, `user2`, `user3`).

## Lesson 05: Modeling the Domain

This lesson is implemented on the branch `lesson-05-domain-modeling`.

*To begin* solving the lesson, switch to the tag `lesson-05-start`.

```
git checkout lesson-05-start
```

*Be aware* that this action will leave your repo in a detached HEAD state.

*This lesson* is teaching how you can utilize an AI tool to develop an object-oriented domain model, and the corresponding database schema.

*The request* is to define companies owned by the registered user. A company has a name, TIN, and an address.

*Your task* is to add these features:
- Define classes `Company` and `Address`
- Implement the database migration script that creates a database schema named `business`, with tables `Companies` and `Addresses`
- Expose a page to list the companies owned by the logged-in user
- Expose a page to add a new company owned by the logged-in user.

## Lesson 06: Implementing the Persistence Patterns

This lesson is implemented on the branch `lesson-06-persistence-patterns`.

*To begin* solving the lesson, switch to the tag `lesson-06-start`.

```
git checkout lesson-06-start
```

*Be aware* that this action will leave your repo in a detached HEAD state.

*This lesson* is teaching how you can utilize an AI tool to implement some of the most common persistence patterns in a database application: Repository, Unit or Work, and Query.

*The request* is to complete the CRUD operations on companies owned by the currently logged-in user. Great stress is on security: Every action must be secure, in the sense that it only operates on the data owned by the logged-in user.

*Your task* is to add these features:
- Define interfaces IRepository<TModel>, IUnitOfWork, and IQuery<TViewModel>
- Implement concrete classes that use Dapper: UnitOfWork, CompaniesRepository, and CompaniesQuery
- Make sure that all queries and SQL statements incorporate the identity of the currently logged-in user
- Complete the NewCompany and Companies Razor pages, which create and list the companies owned by the logged-in user
- Add a page to edit a company, as well as an action to delete a company
- Implement soft deleting, i.e. a Deleted flag in the Companies table

*To verify* that the task is complete:
- The CLI tool creates all the tables with the right fields in the database
- The application user can create, view, update, and delete a company

## Lesson 07: Test-Driven AI

This lesson is implemented on the branch `lesson-07-test-driven-ai`.

*To begin* solving the lesson, switch to the tag `lesson-07-start`.

```
git checkout lesson-07-start
```

*Be aware* that this action will leave your repo in a detached HEAD state.

*This lesson* is teaching how you can drive the decisions of an AI tool by specifying the unit tests its implementation must satisfy. The process will closely resemble TDD.

*The request* is to implement the model for money before implementing financial requirements in the application. Use the AI tool to generate unit tests from the specification, and then (in the style of TDD), to implement the model classes so that all the unit tests pass.

*The money definition* consists of three types:
- Currency - adheres to the ISO 4217 specification of currencies, so-called ISO-3 currency symbols (e.g. USD, EUR, HUF).
- Money - represents a non-negative money amount in a given currency and with a specific precision
- MoneyBag - represents a set of Moneys in different currencies (useful in accounting)

*The `Currency` type* should satisfy these requirements:
- Always contains a currency symbol aligned with ISO 4217
- Exposes the currency symbol
- It is an immutable type

*The `Money` type* should satisfy these requirements:
- Exposes `Currency`, precision (2, 4, or 6), and the decimal amount (non-negative)
- The amount never has more decimals than the precision
- Adding two `Money`s with the same currency (strict addition) creates another money with the sum of the amounts and the precision being the higher of the two
- Adding two general `Money`s (relaxed addition) creates a `MoneyBag`
- Scaling a `Money` by a decimal factor creates a new `Money` with the scaled amount and the same precision
- It is possible to subtract another `Money` if it has the same `Currency` and its amount is not greater than that of the current `Money`

*The `MoneyBag` type* should satisfy these requirements:
- Exposes a sequence of `Money`s
- It always contains at least one `Money`
- There is only one instance of `Money` per `Currency`
- Adding new `Money` with the unique `Currency` adds that instance to the sequence
- Adding new `Money` with the existing `Currency` retains the sum of two instances, observing the strict summation rules for `Money`
- Folding the `MoneyBag` into a `Money` instance is only possible if it contains a single `Currency`

*To verify* that the task is complete, there should be dozens of unit tests for the money-related classes, all green.

## Lesson 08: Upgrading the Database Schema

This lesson is implemented on the branch `lesson-08-upgrading-schema`.

*To begin* solving the lesson, switch to the tag `lesson-08-start`.

```
git checkout lesson-08-start
```

*Be aware* that this action will leave your repo in a detached HEAD state.

*This lesson* is teaching how to utilize the AI tool to develop the database schema and the domain models in lockstep.

*The request* is to separate the autoincrement IDs used by the database from external systems and Web pages. All externally visible objects should carry a GUID identifier, while the database-related code (UoW, repositories) should operate on autoincrement IDs for the database operations.

*A specific* request is to use GUID external IDs in the `Company` and `Address` domain models.

*To verify* that the task is complete, ensure that no autoincrement ID is visible in any of the Web pages operating on companies.

## Lesson 09: Implementing Polymorphic Models

This lesson is implemented on the branch `lesson-09-polymorphic-models`.

*To begin* solving the lesson, switch to the tag `lesson-09-start`.

```
git checkout lesson-09-start
```

*Be aware* that this action will leave your repo in a detached HEAD state.

*This lesson* is teaching how to implement polymorphic domain and database models with the help of AI.

*The request* is to augment the `Address` and the `Company` types:
- An `Address` may have one or several of these meanings: Headquarters, Billing address, Branch address, Legal address, or Other.
- A `Company` may have multiple addresses, each with its one or more meanings.
- A `Company` can either be owned by the logged-in user, or a partner company.

*A specific* request for the `Address` type is to implement polymorphism via an `enum` (with the `Flags` attribute), which would define the meaning of an address.

*A specific* request for the `Company` type is to define two subtypes: `OwnedCompany` and `PartnerCompany`, each modeling a specific business entity.

*To verify* that the task is complete, ensure these behaviors:
- It is possible to add/edit/delete owned companies separate from partner companies;
- It is possible to add/edit/delete multiple addresses for each company
- Each company address can have multiple meanings