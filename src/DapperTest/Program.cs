using Dapper;
using Microsoft.Data.SqlClient;

using var conn = new SqlConnection("Server=.;Database=Mp3Collection;Trusted_Connection=True;Encrypt=False;");

const string artistQuerySql = """
SELECT 
    * 
FROM 
    Albums a
WHERE 
    a.ArtistId = @ArtistId 
    AND a.Name = @AlbumName
""";

var artistId = Guid.Parse("cb874c31-43d2-401c-83b2-ab1f00928244");
var albumName = "Bad Hair Day";

//
// Anonymous Type argument
//

/* Result:
Anonymous Type: AlbumName = Bad Hair Day 
*/
var anonAlbum = (await conn.QueryAsync<Album>(artistQuerySql, new { artistId, albumName })).Single();
Console.WriteLine($"Anonymous Type: AlbumName = {anonAlbum.Name}");


//
// ValueTuple
//

/* Result:
Unhandled exception. System.NotSupportedException: ValueTuple should not be used for parameters - the language-level names are not available to use as parameter names, and it adds unnecessary boxing
   at Dapper.SqlMapper.CreateParamInfoGenerator(Identity identity, Boolean checkForDuplicates, Boolean removeUnused, IList`1 literals) in /_/Dapper/SqlMapper.cs:line 2504
   at Dapper.SqlMapper.GetCacheInfo(Identity identity, Object exampleParameters, Boolean addToCache) in /_/Dapper/SqlMapper.cs:line 1818
   at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 411
   at Program.<Main>$(String[] args) in C:\Dev\SANDBOX\DapperTest\src\DapperTest\Program.cs:line 32
   at Program.<Main>(String[] args)
*/
var valueTupleAlbum = (await conn.QueryAsync<Album>(artistQuerySql, (artistId, albumName))).Single();
Console.WriteLine($"Tuple: AlbumName = {valueTupleAlbum.Name}");


//
// Classes
//

public class Album
{
    public string Name { get; set; } = null!;
}