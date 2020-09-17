# Plugin.CloudFirestore

A cross platform plugin for Firebase Cloud Firestore. 
A wrapper for [Xamarin.Firebase.iOS.CloudFirestore](https://www.nuget.org/packages/Xamarin.Firebase.iOS.CloudFirestore/) 
and [Xamarin.Firebase.Firestore](https://www.nuget.org/packages/Xamarin.Firebase.Firestore/).

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
* Target framework version needs to be Android 10.0.

## Upgrade from Version 3 to 4
Refer to [Upgrade](Upgrade.md)

## Usage
### Get
```C#
var document = await CrossCloudFirestore.Current
                                        .Instance
                                        .Collection("yourcollection")
                                        .Document("yourdocument")
                                        .GetAsync();

var yourModel = document.ToObject<YourModel>();

var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection("yourcollection")
                                     .WhereGreaterThan("Value", 100)
                                     .OrderBy("Value", false)
                                     .LimitTo(3)
                                     .GetAsync();

var yourModels = query.ToObjects<YourModel>();

var group = await CrossCloudFirestore.Current
                                     .Instance
                                     .CollectionGroup("yourcollection")
                                     .GetAsync();

var yourModels = group.ToObjects<YourModel>();
```

### Filters
#### WhereEqualsTo
```C#
var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection("yourcollection")
                                     .WhereEqualsTo("Value", 100)
                                     .GetAsync();                                
```
#### WhereGreaterThan
```C#
var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection("yourcollection")
                                     .WhereGreaterThan("Value", 100)
                                     .GetAsync();                                
```
#### WhereGreaterThanOrEqualsTo
```C#
var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection("yourcollection")
                                     .WhereGreaterThanOrEqualsTo("Value", 100)
                                     .GetAsync();                                
```

#### WhereLessThan
```C#
var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection("yourcollection")
                                     .WhereLessThan("Value", 100)
                                     .GetAsync();                                
```

#### WhereLessThanOrEqualsTo
```C#
var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection("yourcollection")
                                     .WhereLessThanOrEqualsTo("Value", 100)
                                     .GetAsync();                                
```

#### WhereArrayContains
```C#
var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection("yourcollection")
                                     .WhereArrayContains("Values", 100)
                                     .GetAsync();                                
```

#### WhereArrayContainsAny
```C#
var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection("yourcollection")
                                     .WhereArrayContainsAny("Values", new object[] { 100, 200 })
                                     .GetAsync();                                
```

#### WhereIn
```C#
var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection("yourcollection")
                                     .WhereIn("Value", new object[] { 100, 200 })
                                     .GetAsync();                                
```

### Add
```C#
await CrossCloudFirestore.Current
                         .Instance
                         .Collection("yourcollection")
                         .AddAsync(new YourModel());
```
### Update
```C#
await CrossCloudFirestore.Current
                         .Instance
                         .Collection("yourcollection")
                         .Document("yourdocument")
                         .UpdateAsync(new { Value = 10 });
```

### Set
```C#
await CrossCloudFirestore.Current
                         .Instance
                         .Collection("yourcollection")
                         .Document("yourdocument")
                         .SetAsync(new { Value = 1 });

// Create a document with auto-generated ID
await CrossCloudFirestore.Current
                         .Instance
                         .Collection("yourcollection")
                         .Document()
                         .SetAsync(new { Value = 2 });
```

### Delete
```C#
await CrossCloudFirestore.Current
                         .Instance
                         .Collection("yourcollection")
                         .Document("yourdocument")
                         .DeleteAsync();
```

### Transaction
```C#
var reference = CrossCloudFirestore.Current
                                   .Instance
                                   .Collection("yourcollection")
                                   .Document("yourdocument");

await CrossCloudFirestore.Current.Instance.RunTransactionAsync((transaction) =>
{
    var document = transaction.Get(reference);
    var yourModel = document.ToObject<YourModel>();
    
    yourModel.Value++;
    
    transaction.Update(reference, yourModel);
});
```

### Batch
```C#
var reference1 = CrossCloudFirestore.Current
                                    .Instance
                                    .Collection("yourcollection")
                                    .Document("yourdocument1");

var reference2 = CrossCloudFirestore.Current
                                    .Instance
                                    .Collection("yourcollection")
                                    .Document("yourdocument2");
                                    
var reference3 = CrossCloudFirestore.Current
                                    .Instance
                                    .Collection("yourcollection")
                                    .Document("yourdocument3");

await CrossCloudFirestore.Current
                         .Instance
                         .Batch()
                         .Set(reference1, new YourModel())
                         .Update(reference2, "Value", 100)
                         .Delete(reference3)
                         .CommitAsync();
```

### Realtime Update
```C#
CrossCloudFirestore.Current.Instance.Collection("yourcollection")
                           .Document("yourdocument")
                           .AddSnapshotListener((snapshot, error) =>
                           {
                               ...
                           });

CrossCloudFirestore.Current
                   .Instance
                   .Collection("yourcollection")
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
                           .Collection("yourcollection")
                           .Document("yourdocument")
                           .AsObservable()
                           .Subscribe(document =>
                           {
                               ...
                           });
// Added                        
CrossCloudFirestore.Current
                   .Instance
                   .Collection("yourcollection")
                   .ObserveAdded()
                   .Subscribe(documentChange =>
                   {
                       var document = documentChange.Document
                       ...
                   });

// Modified  
CrossCloudFirestore.Current
                   .Instance
                   .Collection("yourcollection")
                   .ObserveModified()
                   .Subscribe(documentChange =>
                   {
                       var document = documentChange.Document
                       ...
                   });

// Removed 
CrossCloudFirestore.Current
                   .Instance
                   .Collection("yourcollection")
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
                                     .Collection("yourcollection")
                                     .WhereEqualsTo(FieldPath.DocumentId, "yourdocument")
                                     .GetDocumentsAsync();
                                     
await CrossCloudFirestore.Current
                         .Instance
                         .Collection("yourcollection")
                         .Document("yourdocument")
                         .UpdateAsync(new FieldPath("a", "b"), 1);
                         
// Create From Class Properties
var fieldPath = FieldPath.CreateFrom((YourModel model) => model.A.B);
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
                         .Collection("yourcollection")
                         .Document("yourdocument")
                         .UpdateDataAsync(data);
```

### Use Multiple Projects
```C#
var document = await CrossCloudFirestore.Current
                                        .GetInstance("SecondAppName")
                                        .Collection("yourcollection")
                                        .Document("yourdocument")
                                        .GetAsync();
```

### Settings
```C#
CrossCloudFirestore.Current.Instance.FirestoreSettings = new FirestoreSettings
{
    CacheSizeBytes = FirestoreSettings.CacheSizeUnlimited
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
                         .Collection("yourcollection")
                         .DocumentAsync(new YourModel());
```
### Support Data Type
| Firestore Type | .NET Type |
|---|---|
| Array | System.Collections.IEnumerable |
| Boolean | bool and bool? |
| Byte | byte[] and Stream |
| Date and time | DateTime, DateTimeOffset, Plugin.CloudFirestore.Timestamp and these Nullable |
| Floating-point number | float, double, decimal and these Nullable |
| Geographical point | Plugin.CloudFirestore.GeoPoint |
| Integer | byte, sbyte, short, ushort, int, uint, long, ulong, enum and these Nullable |
| Map | System.Collections.IDictionary and Any Class　|
| Null | null |
| Reference | Plugin.CloudFirestore.IDocumentReference |
| Text string | string, char, char?, Guid and Guid? |

### Attribute
You can specify attributes to data class properties and fields.
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
    
    [DocumentConverter(typeof(EnumStringConverter))]
    public MyEnum Enum { get; set; }
    
    [DocumentConverter(typeof(CustomConverter), 1)]
    public YourModel Model { get; set; }
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

#### DocumentConverterAttribute
Value is converted by specified converter. You can cpecify your custom converter. Your custom converter must inherit from `DocumentConverter`. 

For example:
```C#
public class CustomConverter : DocumentConverter<int>
{
    private readonly int _parameter;

    public CustomConverter(Type targetType, int arg1) : base(targetType, arg1)
    {
        _parameter = arg1;
    }

    public override bool ConvertFrom(DocumentObject value, out object? result)
    {
        if (value.Type == DocumentObjectType.String)
        {
            result = new YourModel(value.String, _parameter);
            return true;
        }
        return false;
    }

    public override bool ConvertTo(object? value, out object? result)
    {
        if (value is YourModel yourModel)
        {
            result = yourModel.ToString(_parameter);
            return true;
        }
        return false;
    }
}
```

This libray has the following converters.
* `EnumStringConverter` - cnverts enum to string
