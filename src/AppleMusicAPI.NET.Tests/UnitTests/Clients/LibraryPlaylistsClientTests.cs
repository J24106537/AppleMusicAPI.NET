﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AppleMusicAPI.NET.Clients;
using AppleMusicAPI.NET.Extensions;
using AppleMusicAPI.NET.Models.Enums;
using AppleMusicAPI.NET.Models.Requests;
using AutoFixture;
using Moq;
using Xunit;

namespace AppleMusicAPI.NET.Tests.UnitTests.Clients
{
    [Trait("Category", "LibraryPlaylistsClient")]
    public class LibraryPlaylistsClientTests : ClientsTestBase<LibraryPlaylistsClient>
    {
        public class CreateLibraryPlaylist : LibraryPlaylistsClientTests
        {
            public static IEnumerable<object[]> LibraryPlaylistRelationships => AllEnumsMemberData<LibraryPlaylistRelationship>();

            protected LibraryPlaylistCreationRequest Request { get; set; }

            public CreateLibraryPlaylist()
            {
                Request = Fixture.Create<LibraryPlaylistCreationRequest>();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Request = null;
                }

                base.Dispose(disposing);
            }

            [Theory(Skip = "NeedToFixToMockSerialize")]
            [MemberData(nameof(LibraryPlaylistRelationships))]
            public async Task ValidRelationship_IsAddedToQuery(LibraryPlaylistRelationship relationship)
            {
                // Arrange

                // Act
                await Client.CreateLibraryPlaylist(Request, new []{ relationship });

                // Assert
                VerifyHttpClientHandlerSendAsync(Times.Once(), x => x.RequestUri.Query.Equals($"?include={relationship.GetValue()}"));
            }

            [Fact(Skip = "NeedToFixToMockSerialize")]
            public async Task MultipleValidRelationships_AreAddedToQuery()
            {
                // Arrange - Currently only one relationship
                var relationships = new List<LibraryPlaylistRelationship>
                {
                    LibraryPlaylistRelationship.Tracks,
                    LibraryPlaylistRelationship.Tracks
                };

                // Act
                await Client.CreateLibraryPlaylist(Request, relationships);

                // Assert
                VerifyHttpClientHandlerSendAsync(Times.Once(), x => x.RequestUri.Query.Equals($"?include={LibraryPlaylistRelationship.Tracks.GetValue()},{LibraryPlaylistRelationship.Tracks.GetValue()}"));
            }

            [Fact(Skip = "NeedToFixToMockSerialize")]
            public async Task WithValidParameters_AbsolutePathIsCorrect()
            {
                // Arrange

                // Act
                await Client.CreateLibraryPlaylist(Request);

                // Assert
                VerifyHttpClientHandlerSendAsync(Times.Once(), x => x.RequestUri.AbsolutePath.Equals("/v1/me/library/playlists"));
            }
        }
    }
}
