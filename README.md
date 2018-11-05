# Plugin.CloudFirestore

A cross platform plugin for Firebase Cloud Firestore. 
A wrapper for [Xamarin.Firebase.iOS.CloudFirestore](https://www.nuget.org/packages/Xamarin.Firebase.iOS.CloudFirestore/) 
and [Xamarin.Firebase.Firestore](Xamarin.Firebase.Firestore).

## Setup
Install Nuget package to each projects.

[Plugin.CloudFirestore](https://www.nuget.org/packages/Plugin.CloudFirestore/) [![NuGet](https://img.shields.io/nuget/v/Plugin.CloudFirestore.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.CloudFirestore/)

### iOS
* Add GoogleService-Info.plist to iOS project. Select BundleResource as build action.
* Initialize as follows in AppDelegate. 
```C#
Firebase.Core.App.Configure();
```

### Android
* Add google-services.json to Android project. Select GoogleServicesJson as build action. (If you can't select GoogleServicesJson, reload this android project.)
* Initialize as follows in MainActivity.
```C#
Plugin.CloudFirestore.CloudFirestore.Init(this);
```

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
                         .UpdateDataAsync(new Dictionary<string, object> { ["Value"] = 20 });
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
                               //...
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
                               //...
                           });
// Added                        
CrossCloudFirestore.Current
                   .Instance
                   .GetCollection("yourcollection")
                   .ObserveAdded()
                   .Subscribe(document =>
                   {
                       //...
                   });

// Modified  
CrossCloudFirestore.Current
                   .Instance
                   .GetCollection("yourcollection")
                   .ObserveModified()
                   .Subscribe(document =>
                   {
                       //...
                   });

// Removed 
CrossCloudFirestore.Current
                   .Instance
                   .GetCollection("yourcollection")
                   .ObserveRemoved()
                   .Subscribe(document =>
                   {
                       //...
                   });
```

### Use Multiple Projects
```C#
var document = await CrossCloudFirestore.Current
                                        .GetInstance("SecondAppName")
                                        .GetCollection("yourcollection")
                                        .GetDocument("yourdocument")
                                        .GetDocumentAsync();
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
| Date and time | DateTime, DateTimeOffset and these Nullable |
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
}

```
#### IdAttribute
Document id is set when mapping from native. When mapping to native, specified properties are ignored.

#### MapToAttribute
Specified name becomes Firestore field name.

#### IgnoredAttribute
Specified properties are ignored for mapping.
