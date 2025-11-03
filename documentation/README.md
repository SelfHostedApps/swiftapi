# swiftapi
Api swift for the class projects,
allows multiple people to share and complete tasks

---

## Api Route documentation
Jwt based Authentification REST architecture. 
Every routes require jwt except login and SignUp

<table>
<tr>
<td>
**code stack**
- swift(Frontend)
- C#.net8(Backend)
- PostGres
</td>
<td>
**other**
- bash scripting    
- ubuntu(server)
- git/github (versionning)
- podman (containerizing)
- jenkins(ci/cd)
</td>
</tr>
</table>


### Authentification
#### `POST /user/login`
**description:**
login gives the jwt token which is require to have to use the rest of routes.

**Parameters:** email, password

**Response:**
| type | data |
|------|------|
| 200 | "token": ...(jwt) |
| 500 | internal errors OR duplicate data|

---
#### `POST /user/signup`
**description:**
register an account

**Parameters:** email, username, password

**Response:**
| type | data |
|------|------|
| 200 | () |
| 401 | Invalid Credentials |
| 500 | internal errors |


### User Service
#### `GET /user/{username}`
**description:**
Gets a user with is username

**Parameters:** email, username, password
**HEADER:** Bearer jwt-token

**Response:**
| type | data |
|------|------|
| 200 | "user": {email, username, preference} |
| 404 | user not found |

---
#### `DELETE /user/delete`
**description:**
Delete a user requires is password to work

**Parameters:** password
**HEADER:** Bearer jwt-token

**Response:**
| type | data |
|------|------|
| 200 | () |
| 404 | user not found (when api token doesnt have a valid email) |
| 500 | internal errors |

---
#### `PATCH /user/update`
**description:**
update preference of the user own accounts

**Parameters:** password preference
**HEADER:** Bearer jwt-token

**Response:**
| type | data |
|------|------|
| 200 | () |
| 404 | Invalid Credentials |
| 500 | internal errors |



### Task Service
#### `POST /task/create`
**description:**
Creates a task

**Parameters:** description
**HEADER:** Bearer jwt-token

**Response:**
| type | data |
|------|------|
| 200 | () |
| 401 | unauthorize (if token is expired) |
| 404 | user not found |

---
#### `PATCH /task/completed/{id}`
**description:**
Updates a task to complete

**Parameters:** id (route)
**HEADER:** Bearer jwt-token

**Response:**
| type | data |
|------|------|
| 200 | () |
| 401 | id not found |
| 500 | internal errors |

---
#### `GET /task/all/completed`
**description:**
get completed task

**Parameters:** nothing
**HEADER:** Bearer jwt-token

**Response:**
| type | data |
|------|------|
| 200 | "tasks": { id, description, completed, DateTime, user_id } } |
| 404 | Unauthorize acces (Invalid token) |

---
#### `GET /task/all/noncompleted`
**description:**
Gets all noncompleted task

**Parameters:** nothing
**HEADER:** Bearer jwt-token

**Response:**
| type | data |
|------|------|
| 200 | "task": { id, description, completed, DateTime, user_id } |
| 404 | Unauthorize acces (Invalid token) |

---
#### `DELETE /task/email/{email}`
**description:**
find every task associated with an email

**Parameters:** email (route)
**HEADER:** Bearer jwt-token

**Response:**
| type | data |
|------|------|
| 200 | "tasks": { { id, description, completed, DateTime, user_id } } |
| 404 | Unauthorize acces (Invalid token) |

---
#### `GET /task/all/{min}/{max}`
**description:**
find all task between both min and max

**Parameters:** min (route) max (route)
**HEADER:** Bearer jwt-token

**Response:**
| type | data |
|------|------|
| 200 | "tasks": { { id, description, completed, DateTime, user_id } } |
| 404 | Unauthorize acces (Invalid token) |

---
#### `PATCH /task/date/weekly`
**description:**
find all token for the last week

**Parameters:** nothing
**HEADER:** Bearer jwt-token

**Response:**
| type | data |
|------|------|
| 200 | "tasks": { { id, description, completed, DateTime, user_id } } |
| 404 | Unauthorize acces (Invalid token) |







