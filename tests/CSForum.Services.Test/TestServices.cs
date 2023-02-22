using AutoFixture;
using CSForum.Core.Models;
using CSForum.Core.Service;
using FluentAssertions;
using Moq;
using TestLibrary;

namespace CSForum.Services.Test;

public class TestServices
{
    private readonly Mock<IChatService> _chatService = new Mock<IChatService>();
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public async Task ChatService_AddMessage_ReturnNewMessage()
    {
        var message = _fixture.CreateEntityWithoutThrowingRecursionError<Message>();

        _chatService.Setup(x => x.AddMessageAsync(message, 1)).ReturnsAsync(message);

        var messageEntity = await _chatService.Object.AddMessageAsync(message, 1);

        messageEntity.Should().Be(message);
    }
}