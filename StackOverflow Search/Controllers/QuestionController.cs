using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using StackOverflow_Search.Models;

namespace StackOverflow_Search.Controllers
{
    [Route("api/Question")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;

        public QuestionController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        [HttpGet("SearchQbyWord")]
        public IActionResult Search(string Keyword)
        {
            var searchResponse = _elasticClient.Search<Post>(s => s
            .Query(q => q.Match(m => m
            .Field(f => f.Body)
            .Query(Keyword))));

            return Ok(searchResponse.Documents);
        }

        [HttpGet("SearchQbyId")]
        public IActionResult Search(int Id)
        {
            var searchResponse = _elasticClient.Search<Post>(s => s
            .Query(q => q.Term(p => p.Id, Id)));

            return Ok(searchResponse.Documents);
        }

        [HttpGet("SearchQNoTag")]
        public IActionResult Search()
        {
            var searchResponse = _elasticClient.Search<Post>(s => s
            .Query(q => q
            .MatchAll()
             )
            );


            List<int> idsnot = new List<int>();
            foreach(Post x in searchResponse.Documents)
            {
                var post1 = _elasticClient.Search<Post>(s => s.Query(q => q.
                Term(p => p.Id, x.Id) && q.
                TermsSet(c => c
                    .Field(p => p.Body)
                    .Terms(x.Tags)
                ))).Documents;

                if(post1.Count != 0)
                {
                    idsnot.Add(post1.First().Id);
                }
            }

            var finposts = searchResponse.Documents.Where(x => !idsnot.Contains(x.Id));
            return Ok(finposts);
        }

        [HttpPost("SeedAddQ")]
        public async Task<IActionResult> SeedAdd()
        {
            List<Post> Posts = new List<Post>()
            {
                new Post()
                {
                    Id = 0,
                    AcceptedAnswerId = 1,
                    LastActivityDate = DateTime.Now,
                    AnswerCount = 2,
                    Body = "Hello World! Some text...",
                    Score = 1000,
                    ClosedDate = DateTime.Now,
                    CommentCount = 833,
                    ContentLicense = "Creative common",
                    CreationDate = DateTime.Now,
                    CommunityOwnedDate = DateTime.Now,
                    DeletionDate = DateTime.Now,
                    FavoriteCount = 200,
                    LastEditDate = DateTime.Now,
                    ViewCount = 10000,
                    LastEditorDisplayName = "Alex Way",
                    LastEditorUserId = 222,
                    OwnerDisplayName = "Alex",
                    OwnerUserId = 222,
                    PostTypeId = 1,
                    Tags = new List<string>{"one" ,"two" ,"three" },
                    Title = "This is the title of the question."
                },
                new Post()
                {
                    Id = 3,
                    AcceptedAnswerId = 3,
                    LastActivityDate = DateTime.Now,
                    AnswerCount = 3,
                    Body = "New york is here text 2222",
                    Score = 102200,
                    ClosedDate = DateTime.Now,
                    CommentCount = 553,
                    ContentLicense = "Creative common 3",
                    CreationDate = DateTime.Now,
                    CommunityOwnedDate = DateTime.Now,
                    DeletionDate = DateTime.Now,
                    FavoriteCount = 2220,
                    LastEditDate = DateTime.Now,
                    ViewCount = 15522255,
                    LastEditorDisplayName = "Harry Luis",
                    LastEditorUserId = 5,
                    OwnerDisplayName = "W2S",
                    OwnerUserId = 552,
                    PostTypeId = 1,
                    Tags = new List<string>{"I like" ,"cheese!" ,"Rext!" },
                    Title = "Why am I asking this question???222"
                },
                new Post()
                {
                    Id = 2,
                    AcceptedAnswerId = 2,
                    LastActivityDate = DateTime.Now,
                    AnswerCount = 3,
                    Body = "New york is here text",
                    Score = 1000,
                    ClosedDate = DateTime.Now,
                    CommentCount = 83,
                    ContentLicense = "Creative common 3",
                    CreationDate = DateTime.Now,
                    CommunityOwnedDate = DateTime.Now,
                    DeletionDate = DateTime.Now,
                    FavoriteCount = 220,
                    LastEditDate = DateTime.Now,
                    ViewCount = 15555,
                    LastEditorDisplayName = "Gerrard Robs",
                    LastEditorUserId = 123,
                    OwnerDisplayName = "Gerry",
                    OwnerUserId = 321,
                    PostTypeId = 1,
                    Tags = new List<string>{"x" ,"y" , "z", "New york" },
                    Title = "Why am I asking this question???",
                }
            };

            var indexManyAsyncResponse = await _elasticClient.IndexManyAsync(Posts);

            if (indexManyAsyncResponse.Errors)
            {
                foreach (var itemWithError in indexManyAsyncResponse.ItemsWithErrors)
                {
                    Console.WriteLine("Failed to index document {0}: {1}", itemWithError.Id, itemWithError.Error);
                }
                return BadRequest();
            }

            return Ok();
        }
    }
}
