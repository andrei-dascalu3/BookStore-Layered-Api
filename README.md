# BookStore-Layered-Api
Summer practice: Tutorial project for N-tier (layered) API

# Integration tests

## Setup
1. Open SSMS, right click on folder "Databases", click "New Database..."
2. Give name ```BookStoreTest```
3. Click Ok
4. Click "New Query" from top menu in SSMS
5. Run script ```create-tables.sql``` from Scripts folder


## Create and run first integration test
1. Open Visual Studio
2. Go to branch ```integration-tests```
3. Let's write first integration test
4. Run test

## Exercise 1
### Implement ```GetById_WithInvalidId_ShouldReturnNotFound```
- Implement the test to verify that making a ```GET``` request to ```api/books/999``` returns an HTTP ```404 NotFound``` status code

## Exercise 2
### Implement ```Create_WithFuturePublishedDate_ShouldReturnBadRequest```
- Create ```Book``` with date in future
- Call endpoint
- Check endpoint response and database table Book count

