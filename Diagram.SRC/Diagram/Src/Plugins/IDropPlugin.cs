namespace Diagram
{
    public interface IDropPlugin : IDiagramPlugin
    {
        bool DropAction(DiagramView diagramview);
    }
}