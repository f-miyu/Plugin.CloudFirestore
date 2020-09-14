# Upgrade from Version 3 to 4

## Method Name Changes
Some method names have changed. Old methods have become obsolete.

### IFirestore

| Version 3          | Version 4       | 
| ------------------ | --------------- | 
| GetCollection      | Collection      | 
| GetDocument        | Document        | 
| GetCollectionGroup | CollectionGroup | 
| CreateBatch        | Batch           | 

### ICollectionReference

|  Version 3       |  Version 4 | 
| ---------------- | ---------- | 
| CreateDocument   | Document   | 
| GetDocument      | Document   | 
| AddDocumentAsync | AddAsync   | 

### IDocumentReference

|  Version 3          |  Version 4  | 
| ------------------- | ----------- | 
| GetCollection       | Collection  | 
| GetDocumentAsync    | GetAsync    | 
| SetDataAsync        | SetAsync    | 
| UpdateDataAsync     | UpdateAsync | 
| DeleteDocumentAsync | DeleteAsync | 

### IQuery

|  Version 3        |  Version 4 | 
| ----------------- | ---------- | 
| GetDocumentsAsync | GetAsync   | 

### ITransaction

| Version 3      | Version 4 | 
| -------------- | --------- | 
| GetDocument    | Get       | 
| SetData        | Set       | 
| UpdateData     | Update    | 
| DeleteDocument | Delete    | 

### IWriteBatch

| Version 3      | Version 4 | 
| -------------- | --------- | 
| GetDocument    | Get       | 
| SetData        | Set       | 
| UpdateData     | Update    | 
| DeleteDocument | Delete    | 

## Namespace Changes
A namespace has changed.

| Version 3                        | Version 4                      | 
| -------------------------------- | ------------------------------ | 
| Plugin.CloudFirestore.Extensions | Plugin.CloudFirestore.Reactive | 
