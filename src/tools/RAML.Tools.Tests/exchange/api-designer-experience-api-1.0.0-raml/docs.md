# API designer Docs

## 1 - General headers

For every request, send the following headers that you can get from authenticating againts Core Services:
```
-H 'Authorization: Bearer <token>' 
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <user-id>'  
```

---

## 2 - Working with projects

### 2.1 - Listing projects

List all project that user has permissions

```
curl -i https://qax.anypoint.mulesoft.com/designcenter/api-designer/projects 
-H'content-type:application/json' 
-H'Authorization:Bearer <token>' 
-H'x-organization-id: <org-id>' 
-H'x-owner-id: <user-id>'
```

### 2.2 - Creating a project

Send the project name along with the classifier `raml` or `raml-fragment` depending on the type of project you have.
The type can be used in the case of the `raml-fragment`, to specify the concret fragment type.

Creating an spec:
```
curl 'https://qax.anypoint.mulesoft.com/designcenter/api-designer/projects' 
-H 'Authorization: Bearer <token>' 
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <user-id>' 
-H 'accept: application/json'
--data-binary '{"name":"My New Project", "classifier":"raml"}' 
--compressed
```

Creating a fragment:
```
curl 'https://qax.anypoint.mulesoft.com/designcenter/api-designer/projects' 
-H 'Authorization: Bearer <token>' 
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <user-id>' 
-H 'accept: application/json'
--data-binary '{"name":"My New Project", "classifier":"raml-fragment", "subType":"trait"}' 
--compressed
```


Expect a 201 response code, with a json that has an `id` property, representing the project id. 
This `id` can then be used to operate on the project.

### 2.3 - Removing a project

Having the project id, you can delete the project.

``` 
curl 'https://qax.anypoint.mulesoft.com/designcenter/api-designer/projects/<project-id>' 
-X DELETE 
-H 'Authorization: Bearer <token>'
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <user-id>' 
--compressed
```

Expect a 204 response code.

### 2.4 - Getting a project details

```
curl 'https://anypoint.mulesoft.com/designcenter/api-designer/projects/<project-id>' 
-H 'Authorization: Bearer <token>'
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <user-id>' 
-H 'accept: application/json' 
--compressed
```

Expect a `200` with: 

```
{
  id: "<project-id>", 
  name: "The project name",
  createdBy: "<user-id>",
  createdDate: 1505484487103,
  organizationId: "<org-id>"
}
```


---

## 2 - Working with branches

Note that branch `master` is **always** present inside a project, and can't be deleted.

### 2.1 - Listing branches

```
curl 'https://anypoint.mulesoft.com/designcenter/api-designer/projects/<project-id>/branches' 
-H 'Authorization: Bearer <token>' 
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <owner-id>'
-H 'accept: application/json' 
--compressed
```

Expect a 200 response code, with the list of branches:
```
[
  {name: "master", commitId: "<head-commit-sha>"}
]
```

### 2.2 - Creating a new branch

You need to provide a branch name and a commitId from where to branch.
If commitId is not present it will branch from master

```
curl 'https://anypoint.mulesoft.com/designcenter/api-designer/projects/<project-id>/branches' 
-H 'Authorization: Bearer <token>'
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <owner-id>' 
-H 'accept: application/json'
--data-binary '{"name":"my-new-branch-name","commitId":"<base-commit-id>"}' --compressed
```

Expect a 200 response code, with the created of branches:

```
{name: "my-new-branch-name", commitId: "<base-commit-id>"}
```

If branch already exists a 409 response is sent

---

## 3 - Reading branch content 

For all this operations, you will need the project id and the branch name.

### 3.1 - Listing branch files

```
curl 'https://anypoint.mulesoft.com/designcenter/api-designer/projects/<project-id>/branches/<branch>/files' 
-H 'Authorization: Bearer <token>' 
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <owner-id>' 
-H 'accept: application/json' 
--compressed
```

Expect a 200 response code with the list of files and folders in the project:
```
[
  {path: "api.raml", type: "FILE"},
  {path: "example", type: "FOLDER"}
  {path: "example/file.json", type: "FILE"}
]
```

### 3.2 - Reading a file content

`file-path` must be uri complain. `/` in file-path must be escaped

```
curl 'https://anypoint.mulesoft.com/designcenter/api-designer/projects/f9e3b0a7-75eb-4626-9646-2736eb48edc7/branches/master/files/<file-path>' 
-H 'Authorization: Bearer <token>' 
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <owner-id>' 
-H 'Accept: */*' 
--compressed
```

Expect a `200` (or `304`) string of the file content.


---

## 4 - Writing branch content 

For all this operations, you will need the project id and the branch name.

### 4.1 - Acquiring the lock

To avoid edition conflicts, the branches edition has a lock. To edit a branch, you first need to request the edition lock over it.

```
curl 'https://anypoint.mulesoft.com/designcenter/api-designer/projects/<project-id>/branches/<branch>/acquireLock' 
-H 'Authorization: Bearer <token>' 
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <owner-id>' 
-H 'accept: application/json'
--data-binary '{}'
--compressed
```

Expect a `200` with:
```
{ locked: true, name: "<user-name>" }
```

### 4.2 - Creating or updating a file

A list of `path`,`content` is sent to save. If file existis, it is updated, if not, it is created with the `content`

```
curl 'https://anypoint.mulesoft.com/designcenter/api-designer/projects/<project-id>/branches/<branch>/save' 
-H 'Authorization: Bearer <token>'
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <owner-id>' 
-H 'accept: application/json'
--data-binary '[{"path":"/api.raml","type":"FILE","content":"#%RAML 1.0\ntitle: API title"}]'
--compressed
```

Expect a `200` response, with the list of files from the branch.

### 4.3 - Moving or renaming a file

`file-path` must be uri complain. `/` in file-path must be escaped


```
curl 'https://anypoint.mulesoft.com/designcenter/api-designer/projects/<project-id>/branches/<branch>/files/<file-path>/move' 
-H 'Authorization: Bearer <token>'
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <owner-id>' 
-H 'accept: application/json'
--data-binary '{"path":"moved-api.raml"}'
--compressed
```

Expect a `200` response code.

### 4.4 - Deleting a file

`file-path` must be uri complain. `/` in file-path must be escaped

If `file-path` is a folder, it will delete all the content too.

```
curl 'https://anypoint.mulesoft.com/designcenter/api-designer/projects/<project-id>/branches/<branch>/files/<file-path>' 
-X DELETE 
-H 'Authorization: Bearer <token>' 
-H 'x-organization-id: <org-id>'
-H 'x-owner-id: <owner-id>' 
--compressed
```

Expect a `200` response code.

### 4.5 - Mantaining the write lock

The lock over a branch will only lst for a minute, and will expire afterwards.
To mantain the lock over a branch, a call needs to be made.

```
curl 'https://anypoint.mulesoft.com/designcenter/api-designer/projects/<project-id>/branches/<branch>/status'
-H 'Authorization: Bearer <token>' 
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <owner-id>' 
-H 'accept: application/json'
--data-binary '{}' --compressed
```

This will return a `200` with he lock information:
```
{lock: {locked: true, lockedBy: "<my-user-name>", lockedByMe: true}
```

### 4.6 - Releasing the write lock

After edition over the branch is done, the lock can be released manually. If this is not done, the lock will still expire.

```
curl 'https://anypoint.mulesoft.com/designcenter/api-designer/projects/<proejct-id>/branches/<branch>/releaseLock' 
-H 'Authorization: Bearer <token>'
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <owner-id>' 
-H 'accept: application/json'
--data-binary '{}' --compressed
```

Expect a `200` response code.



---

## 5 - Interacting with Exchange

### 5.1 - Publishing

```
curl 'https://anypoint.mulesoft.com/designcenter/api-designer/projects/<proejct-id>/branches/<branch>/publish/exchange' 
-H 'Authorization: Bearer <token>'
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <owner-id>' 
-H 'accept: application/json'
--data-binary '{"name":"ProjectName", "apiVersion":"'0.1'", "version":"1.0.0", "main":"api.raml", "assetId":"myasset", "groupId":"f397ce0e-ba9b-40fb-a96c-8bbc1d494b40","classifier":"raml"}' 
--compressed
```

Expect a `200` response code

```
{"organizationId":"f397ce0e-ba9b-40fb-a96c-8bbc1d494b40","groupId":"f397ce0e-ba9b-40fb-a96c-8bbc1d494b40","assetId":"visualtest","version":"1.0.0"}
```


### 5.2 - Consuming fragments

```
curl 'https://anypoint.mulesoft.com/designcenter/api-designer/exchange/graphql' 
-H 'Authorization: Bearer <token>'
-H 'x-organization-id: <org-id>' 
-H 'x-owner-id: <owner-id>' 
-H 'accept: application/json'
--data-binary '{"query":"{assets(query: {searchTerm: \"\", type: \"raml-fragment\", offset: 0, limit: 20, organizationIds:[\"\"]},latestVersionsOnly: true,) {organizationId, name, rating, numberOfRates, version, groupId, assetId, updatedAt, createdBy { firstName, lastName,  userName }}}",
"variables":"{\"accessToken\": \"<token>\"}",
"operationName":null}' 
--compressed
```

Expect a `200` response code

```
{"data":{"assets":[{"organizationId":"45541135-058b-4bab-9b91-cef18ba31642","name":"trait","rating":0,"numberOfRates":0,"version":"1.0.0","groupId":"f397ce0e-ba9b-40fb-a96c-8bbc1d494b40.api-designer","assetId":"trait","updatedAt":"2017-09-21T18:49:35.504Z","createdBy":{"firstName":"Mozart","lastName":"Tekgenesis","userName":"mozart-tg"}},
{"organizationId":"f397ce0e-ba9b-40fb-a96c-8bbc1d494b40","name":"caro-trait","rating":0,"numberOfRates":0,"version":"1.0.1","groupId":"f397ce0e-ba9b-40fb-a96c-8bbc1d494b40","assetId":"caro-trait","updatedAt":"2017-09-20T12:50:23.78Z","createdBy":{"firstName":"Mozart","lastName":"Tekgenesis","userName":"mozart-tg"}}]}}
```

