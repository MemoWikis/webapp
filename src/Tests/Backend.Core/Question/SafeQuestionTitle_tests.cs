internal class SafeQuestionTitle_tests : BaseTestHarness
{
    [Test]
    public async Task Get_Should_Remove_TipTap_Figure_Elements()
    {
        // Arrange
        var htmlWithTipTapFigure = @"<p>Question text before</p>
            <figure class=""tiptap-figure"" data-caption=""Test caption"" data-license=""Test license"">
                <img src=""test.jpg"" alt=""Test image"" />
                <figcaption class=""tiptap-figcaption"">Test caption</figcaption>
            </figure>
            <p>Question text after</p>";

        // Act
        var result = SafeQuestionTitle.Get(htmlWithTipTapFigure);

        // Assert
        await Verify(result);
    }

    [Test]
    public async Task Get_Should_Leave_Regular_Figure_Elements_Intact()
    {
        // Arrange
        var htmlWithRegularFigure = @"<p>Question text before</p>
            <figure class=""regular-figure"">
                <img src=""test.jpg"" alt=""Test image"" />
                <figcaption>Regular caption</figcaption>
            </figure>
            <p>Question text after</p>";

        // Act
        var result = SafeQuestionTitle.Get(htmlWithRegularFigure);

        // Assert
        await Verify(result);
    }
}
