using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Diagram
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class DiagramTest
    {
        
        public DiagramTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void BasicMethods()
        {

            Diagram diagram = new Diagram();

            // create node
            Position position = new Position(100, 100);
            Node firstNode = diagram.CreateNode(position);

            // check new node position
            Assert.AreEqual(firstNode.position.x, 100, "firstNode bad position");
            Assert.AreEqual(firstNode.position.y, 100, "firstNode bad position");

            // get all nodes
            Nodes nodes = diagram.GetAllNodes();

            // check if node is created
            Assert.AreEqual(nodes.Count, 1, "wrong nodes count");

            // check node position
            Node node = nodes[0];
            Assert.AreEqual(nodes[0].position.x, 100, "node bad position");
            Assert.AreEqual(nodes[0].position.y, 100, "node bad position");

            // create second node
            Position position2 = new Position(200, 200);
            Node secondNode = diagram.CreateNode(position2);

            // check second node position
            Assert.AreEqual(secondNode.position.x, 200, "secondNode bad position");
            Assert.AreEqual(secondNode.position.y, 200, "secondNode bad position");

            // create line
            Line line = diagram.Connect(firstNode, secondNode);
            Assert.IsNotNull(line, "create line failed");

            Lines lines = diagram.GetAllLines();
            Assert.AreEqual(lines.Count, 1, "wrong lines count");
            
            Assert.IsTrue(lines[0].start == firstNode.id, "firstNode invalid id");
            Assert.IsTrue(lines[0].end == secondNode.id, "secondNode invalid id");
        }

        [TestMethod]
        public void CopyPartOfDiagramMethods()
        {
            Diagram diagram = new Diagram();
            Position position = new Position(100, 100);
            Node firstNode = diagram.CreateNode(position);
            Position position2 = new Position(200, 200);
            Node secondNode = diagram.CreateNode(position2);
            Line line = diagram.Connect(firstNode, secondNode);

            Nodes nodes = diagram.GetAllNodes();
            
            string xml = diagram.GetDiagramPart(nodes);

            DiagramBlock newBlock = diagram.AddDiagramPart(xml, new Position(300, 300), 0);            

            Assert.AreEqual(newBlock.nodes.Count, 2, "wrong nodes count");
            Assert.AreEqual(newBlock.lines.Count, 1, "wrong lines count");

            Node firstNodeCopy = newBlock.nodes[0];
            Node secondNodeCopy = newBlock.nodes[1];
            Line lineCopy = newBlock.lines[0];

            Assert.IsTrue(firstNodeCopy.id == 3, "firstNode invalid id");
            Assert.IsTrue(secondNodeCopy.id == 4, "secondNode invalid id");

            // check copied nodes position
            Assert.AreEqual(firstNodeCopy.position.x, 300, "firstNodeCopy bad position");
            Assert.AreEqual(firstNodeCopy.position.y, 300, "firstNodeCopy bad position");
            Assert.AreEqual(secondNodeCopy.position.x, 400, "secondNode bad position");
            Assert.AreEqual(secondNodeCopy.position.y, 400, "secondNode bad position");


            Assert.IsTrue(lineCopy.start == firstNodeCopy.id, "firstNode copy invalid id");
            Assert.IsTrue(lineCopy.end == secondNodeCopy.id, "secondNode copy invalid id");
        }

        [TestMethod]
        public void CopyPartOfDiagramWithLayersMethods()
        {
            Diagram diagram = new Diagram();
            Position position = new Position(100, 100);
            Node firstNode = diagram.CreateNode(position);
            Position position2 = new Position(200, 200);
            Node secondNode = diagram.CreateNode(position2);
            Line line = diagram.Connect(firstNode, secondNode);

            Nodes nodes = diagram.GetAllNodes();

            string xml = diagram.GetDiagramPart(nodes);

            DiagramBlock newBlock = diagram.AddDiagramPart(xml, new Position(300, 300), 0);

            Assert.AreEqual(newBlock.nodes.Count, 2, "wrong nodes count");
            Assert.AreEqual(newBlock.lines.Count, 1, "wrong lines count");

            Node firstNodeCopy = newBlock.nodes[0];
            Node secondNodeCopy = newBlock.nodes[1];
            Line lineCopy = newBlock.lines[0];

            Assert.IsTrue(firstNodeCopy.id == 3, "firstNode invalid id");
            Assert.IsTrue(secondNodeCopy.id == 4, "secondNode invalid id");

            // check copied nodes position
            Assert.AreEqual(firstNodeCopy.position.x, 300, "firstNodeCopy bad position");
            Assert.AreEqual(firstNodeCopy.position.y, 300, "firstNodeCopy bad position");
            Assert.AreEqual(secondNodeCopy.position.x, 400, "secondNode bad position");
            Assert.AreEqual(secondNodeCopy.position.y, 400, "secondNode bad position");


            Assert.IsTrue(lineCopy.start == firstNodeCopy.id, "firstNode copy invalid id");
            Assert.IsTrue(lineCopy.end == secondNodeCopy.id, "secondNode copy invalid id");
        }
    }
}
