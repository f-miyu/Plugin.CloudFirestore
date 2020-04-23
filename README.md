# Plugin.CloudFirestore

A cross platform plugin for Firebase Cloud Firestore. 
A wrapper for [Xamarin.Firebase.iOS.CloudFirestore](https://www.nuget.org/packages/Xamarin.Firebase.iOS.CloudFirestore/) 
and a binding library of firebase-firestore-19.0.2.

## Setup
Install Nuget package to each projects.

[Plugin.CloudFirestore](https://www.nuget.org/packages/Plugin.CloudFirestore/) [![NuGet](https://img.shields.io/nuget/vpre/Plugin.CloudFirestore.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.CloudFirestore/)

### iOS
* Add GoogleService-Info.plist to iOS project. Select BundleResource as build action.
* Initialize as follows in AppDelegate. 
```C#
Firebase.Core.App.Configure();
```

### Android
* Add google-services.json to Android project. Select GoogleServicesJson as build action. (If you can't select GoogleServicesJson, reload this android project.)

## Usage
### Get
```C#
var document = await CrossCloudFirestore.Current
                                        .Instance
                                        .GetCollection("yourcollection")
                                        .GetDocument("yourdocument")
                                        .GetDocumentAsync();

var yourModel = document.ToObject<YourModel>();

var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection("yourcollection")
                                     .WhereGreaterThan("Value", 100)
                                     .OrderBy("Value", false)
                                     .LimitTo(3)
                                     .GetDocumentsAsync();

var yourModels = query.ToObjects<YourModel>();

var group = await CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollectionGroup("yourcollection")
                                     .GetDocumentsAsync();

var yourModels = group.ToObjects<YourModel>();
```

### Filters
#### WhereEqualsTo
```C#
var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection("yourcollection")
                                     .WhereEqualsTo("Value", 100)
                                     .GetDocumentsAsync();                                
```
#### WhereGreaterThan
```C#
var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection("yourcollection")
                                     .WhereGreaterThan("Value", 100)
                                     .GetDocumentsAsync();                                
```
#### WhereGreaterThanOrEqualsTo
```C#
var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection("yourcollection")
                                     .WhereGreaterThanOrEqualsTo("Value", 100)
                                     .GetDocumentsAsync();                                
```

#### WhereLessThan
```C#
var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection("yourcollection")
                                     .WhereLessThan("Value", 100)
                                     .GetDocumentsAsync();                                
```

#### WhereLessThanOrEqualsTo
```C#
var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection("yourcollection")
                                     .WhereLessThanOrEqualsTo("Value", 100)
                                     .GetDocumentsAsync();                                
```

#### WhereArrayContains
```C#
var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection("yourcollection")
                                     .WhereArrayContains("Values", 100)
                                     .GetDocumentsAsync();                                
```

### Add
```C#
await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("yourcollection")
                         .AddDocumentAsync(new YourModel());
```
### Update
```C#
await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("yourcollection")
                         .GetDocument("yourdocument")
                         .UpdateDataAsync(new { Value = 10 });
```

### Set
```C#
await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("yourcollection")
                         .GetDocument("yourdocument")
                         .SetDataAsync(new { Value = 1 });

// Create a document with auto-generated ID
await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("yourcollection")
                         .CreateDocument()
                         .SetDataAsync(new { Value = 2 });
```

### Delete
```C#
await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("yourcollection")
                         .GetDocument("yourdocument")
                         .DeleteDocumentAsync();
```

### Transaction
```C#
var reference = CrossCloudFirestore.Current
                                   .Instance
                                   .GetCollection("yourcollection")
                                   .GetDocument("yourdocument");

await CrossCloudFirestore.Current.Instance.RunTransactionAsync((transaction) =>
{
    var document = transaction.GetDocument(reference);
    var yourModel = document.ToObject<YourModel>();
    
    yourModel.Value++;
    
    transaction.UpdateData(reference, yourModel);
});
```

### Batch
```C#
var batch = CrossCloudFirestore.Current.Instance.CreateBatch();

var reference = CrossCloudFirestore.Current
                                   .Instance
                                   .GetCollection("yourcollection")
                                   .GetDocument("yourdocument");

var yourModel = new YourModel();

batch.SetData(reference, yourModel);

yourModel.Value++;

batch.UpdateData(reference, yourModel);

await batch.CommitAsync();
```

### Realtime Update
```C#
CrossCloudFirestore.Current.Instance.GetCollection("yourcollection")
                           .GetDocument("yourdocument")
                           .AddSnapshotListener((snapshot, error) =>
                           {
                               ...
                           });

CrossCloudFirestore.Current
                   .Instance
                   .GetCollection("yourcollection")
                   .AddSnapshotListener((snapshot, error) =>
                   {
                       if(snapshot != null) 
                       {
                           foreach (var documentChange in snapshot.DocumentChanges)
                           {
                               switch (documentChange.Type)
                               {
                                   case DocumentChangeType.Added:
                                       // Document Added
                                       break;
                                   case DocumentChangeType.Modified:
                                       // Document Modified
                                       break;
                                   case DocumentChangeType.Removed:
                                       // Document Removed
                                       break;
                                }
                            }
                        }
                    });
```
Use of Reactive Extensions
```C#
CrossCloudFirestore.Current.Instance
                           .GetCollection("yourcollection")
                           .GetDocument("yourdocument")
                           .AsObservable()
                           .Subscribe(document =>
                           {
                               ...
                           });
// Added                        
CrossCloudFirestore.Current
                   .Instance
                   .GetCollection("yourcollection")
                   .ObserveAdded()
                   .Subscribe(documentChange =>
                   {
                       var document = documentChange.Document
                       ...
                   });

// Modified  
CrossCloudFirestore.Current
                   .Instance
                   .GetCollection("yourcollection")
                   .ObserveModified()
                   .Subscribe(documentChange =>
                   {
                       var document = documentChange.Document
                       ...
                   });

// Removed 
CrossCloudFirestore.Current
                   .Instance
                   .GetCollection("yourcollection")
                   .ObserveRemoved()
                   .Subscribe(documentChange =>
                   {
                       var document = documentChange.Document
                       ...
                   });
```

### FieldPath
```C#
var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection("yourcollection")
                                     .WhereEqualsTo(FieldPath.DocumentId, "yourdocument")
                                     .GetDocumentsAsync();
                                     
await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("yourcollection")
                         .GetDocument("yourdocument")
                         .UpdateDataAsync(new FieldPath("a", "b"), 1);
```

### FieldValue
```C#
var data = new
{
    DeletedValue = FieldValue.Delete,
    Date = FieldValue.ServerTimestamp,
    UnionArray = FieldValue.ArrayUnion(1, 2),
    RemovedArray = FieldValue.ArrayRemove(3, 4),
    LongValue = FieldValue.Increment(1),
    DoubleValue = FieldValue.Increment(2.5),
};

await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("yourcollection")
                         .GetDocument("yourdocument")
                         .UpdateDataAsync(data);
```

### Use Multiple Projects
```C#
var document = await CrossCloudFirestore.Current
                                        .GetInstance("SecondAppName")
                                        .GetCollection("yourcollection")
                                        .GetDocument("yourdocument")
                                        .GetDocumentAsync();
```

### Settings
```C#
CrossCloudFirestore.Current.Instance.FirestoreSettings = new FirestoreSettings
{
    AreTimestampsInSnapshotsEnabled = true
};
```

## Data Mapping
This library can map native data to .NET data and .NET data to native data.
```C#
// native to .NET
var yourModel = document.ToObject<YourModel>();

// .NET to native
await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("yourcollection")
                         .AddDocumentAsync(new YourModel());
```
### Support Data Type
| Firestore Type | .NET Type |
|---|---|
| Array | System.Collections.IList |
| Boolean | bool and bool? |
| Byte | byte[] and Stream |
| Date and time | DateTime, DateTimeOffset, Plugin.CloudFirestore.Timestamp and these Nullable |
| Floating-point number | float, double, decimal and these Nullable |
| Geographical point | Plugin.CloudFirestore.GeoPoint |
| Integer | byte, sbyte, short, ushort, int, uint, long, ulong and these Nullable |
| Map | System.Collections.IDictionary and Any Class　|
| Null | null |
| Reference | Plugin.CloudFirestore.IDocumentReference |
| Text string | string |

### Attribute
You can specify attributes to data class properties.
```C#
public class YourModel 
{
    [Id]
    public string Id { get; set; }
    
    [MapTo("value")]
    public int Value { get; set; }
    
    [Ignored]
    public string Text { get; set; }
    
    [ServerTimestamp(CanReplace = false)]
    public Timestamp CreatedAt { get; set; }

    [ServerTimestamp]
    public Timestamp UpdatedAt { get; set; }
}

```
#### IdAttribute
Document id is set when mapping from native. When mapping to native, specified properties are ignored.

#### MapToAttribute
Specified name becomes Firestore field name.

#### IgnoredAttribute
Specified properties are ignored for mapping.

#### ServerTimestampAttribute
Specified properties are replaced to a server timestamp. If CanReplace is false, the properties are replaced when they are default or null. The default value for CanReplace is true.
