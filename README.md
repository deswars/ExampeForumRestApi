# ForumRestApiExample
## Rest Api
- categories
  - /api/categories
    - GET - list of categories
    - POST - add new category
  - /api/categories/{id}
    - GET - get category
    - PUT - update category 
    - DELETE - delete category including all dependent topics and messages
  - /api/categories/{id}/topics
    - GET - get all topics in category
  - /api/categories/{id}/topics/page/{pageNumber}
    - GET - get selected page of all topics in category

- topics
  - /api/topics
    - POST - add new topic
  - /api/topics/{id}
    - GET - get topic data
    - PUT - update topic
    - DELETE - delete topic including all dependent messages
  - /api/topics/{id}/messages
    - GET - gat all messages inside topic
  - /api/topics/{id}/messages/page/{pageNumber}
    - GET - get selected page of all messages in topic

- messages
  - /api/messages
    - POST - add new message
  - /api/messages/{id}
    - GET - get message data
    - PUT - update message
    - DELETE - delete message
  - /api/messages/{id}/author
    - GET - get author of message

- users
  - /api/users
    - GET - get all users
    - POST - add new user
  - /api/users/{id}
    - GET - get user data
    - PUT - update user
    - DELETE - delete user
  - /api/users/{id}/messages
  	- GET - get all user messages
  - /api/users/{id}messages/last/{pagenumber}
    - GET - get selected page of all user messages startinf from newest


##Database Stucture

User
Id|Name|Status|PSText
--|--|--|--
long|string|enum|string

Category
Id|Name|Status
--|--|--
long|string|enum

Topic
Id|Name|Category_id|Status
--|--|--|--
long|string|long|enum

Message
Id|Text|Author_id|Create_date|Edit_date|Status
--|--|--|--|--|--
long|string|long|date|date|enum
