using OllPractice.Model.Model;
using Xunit;

namespace OllPracticeTests
{
    public class CubeTurnTests
    {
        [Fact]
        public void TestCubeCreation()
        {
            //Arrange
            var cube = new Cube();

            //Act

            //Assert
            Assert.Equal($"WWW\r\nW W\r\nWWW", cube.Faces[0].PrintSide());
            Assert.Equal($"GGG\r\nG G\r\nGGG", cube.Faces[1].PrintSide());
            Assert.Equal($"OOO\r\nO O\r\nOOO", cube.Faces[2].PrintSide());
            Assert.Equal($"RRR\r\nR R\r\nRRR", cube.Faces[3].PrintSide());
            Assert.Equal($"BBB\r\nB B\r\nBBB", cube.Faces[4].PrintSide());
            Assert.Equal($"YYY\r\nY Y\r\nYYY", cube.Faces[5].PrintSide());
        }

        [Fact]
        public void TestCubeTurnU()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.U();

            //Assert
            Assert.Equal($"WWW\r\nW W\r\nWWW", cube.Faces[0].PrintSide());
            Assert.Equal($"RRR\r\nG G\r\nGGG", cube.Faces[1].PrintSide());
            Assert.Equal($"GGG\r\nO O\r\nOOO", cube.Faces[2].PrintSide());
            Assert.Equal($"BBB\r\nR R\r\nRRR", cube.Faces[3].PrintSide());
            Assert.Equal($"OOO\r\nB B\r\nBBB", cube.Faces[4].PrintSide());
            Assert.Equal($"YYY\r\nY Y\r\nYYY", cube.Faces[5].PrintSide());
        }

        [Fact]
        public void TestCubeTurnUPrime()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.UPrime();

            //Assert
            Assert.Equal($"WWW\r\nW W\r\nWWW", cube.Faces[0].PrintSide());
            Assert.Equal($"OOO\r\nG G\r\nGGG", cube.Faces[1].PrintSide());
            Assert.Equal($"BBB\r\nO O\r\nOOO", cube.Faces[2].PrintSide());
            Assert.Equal($"GGG\r\nR R\r\nRRR", cube.Faces[3].PrintSide());
            Assert.Equal($"RRR\r\nB B\r\nBBB", cube.Faces[4].PrintSide());
            Assert.Equal($"YYY\r\nY Y\r\nYYY", cube.Faces[5].PrintSide());
        }
        [Fact]
        public void TestCubeTurnR()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.R();

            //Assert
            Assert.Equal($"WWG\r\nW G\r\nWWG", cube.Faces[0].PrintSide());
            Assert.Equal($"GGY\r\nG Y\r\nGGY", cube.Faces[1].PrintSide());
            Assert.Equal($"OOO\r\nO O\r\nOOO", cube.Faces[2].PrintSide());
            Assert.Equal($"RRR\r\nR R\r\nRRR", cube.Faces[3].PrintSide());
            Assert.Equal($"WBB\r\nW B\r\nWBB", cube.Faces[4].PrintSide());
            Assert.Equal($"BYY\r\nB Y\r\nBYY", cube.Faces[5].PrintSide());
        }
        [Fact]
        public void TestCubeTurnRPrime()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.RPrime();

            //Assert
            Assert.Equal($"WWB\r\nW B\r\nWWB", cube.Faces[0].PrintSide());
            Assert.Equal($"GGW\r\nG W\r\nGGW", cube.Faces[1].PrintSide());
            Assert.Equal($"OOO\r\nO O\r\nOOO", cube.Faces[2].PrintSide());
            Assert.Equal($"RRR\r\nR R\r\nRRR", cube.Faces[3].PrintSide());
            Assert.Equal($"YBB\r\nY B\r\nYBB", cube.Faces[4].PrintSide());
            Assert.Equal($"GYY\r\nG Y\r\nGYY", cube.Faces[5].PrintSide());
        }

        [Fact]

        public void TestCubeTurnRURU()
        { 
            //Arrange
            var cube = new Cube();

            //Act
            cube.R();
            cube.U();
            cube.RPrime();
            cube.UPrime();

            //Assert
            Assert.Equal($"WWO\r\nW G\r\nWWG", cube.Faces[0].PrintSide());
            Assert.Equal($"GGY\r\nG W\r\nGGG", cube.Faces[1].PrintSide());
            Assert.Equal($"BOO\r\nO O\r\nOOO", cube.Faces[2].PrintSide());
            Assert.Equal($"RRW\r\nB R\r\nWRR", cube.Faces[3].PrintSide());
            Assert.Equal($"BRR\r\nB B\r\nBBB", cube.Faces[4].PrintSide());
            Assert.Equal($"YYY\r\nY Y\r\nRYY", cube.Faces[5].PrintSide());

        }
        [Fact]
        public void TestCubeTurnL()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.L();

            //Assert
            Assert.Equal($"BWW\r\nB W\r\nBWW", cube.Faces[0].PrintSide());
            Assert.Equal($"WGG\r\nW G\r\nWGG", cube.Faces[1].PrintSide());
            Assert.Equal($"OOO\r\nO O\r\nOOO", cube.Faces[2].PrintSide());
            Assert.Equal($"RRR\r\nR R\r\nRRR", cube.Faces[3].PrintSide());
            Assert.Equal($"BBY\r\nB Y\r\nBBY", cube.Faces[4].PrintSide());
            Assert.Equal($"YYG\r\nY G\r\nYYG", cube.Faces[5].PrintSide());
        }

        [Fact]
        public void TestCubeTurnLPrime()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.LPrime();

            //Assert
            Assert.Equal($"GWW\r\nG W\r\nGWW", cube.Faces[0].PrintSide());
            Assert.Equal($"YGG\r\nY G\r\nYGG", cube.Faces[1].PrintSide());
            Assert.Equal($"OOO\r\nO O\r\nOOO", cube.Faces[2].PrintSide());
            Assert.Equal($"RRR\r\nR R\r\nRRR", cube.Faces[3].PrintSide());
            Assert.Equal($"BBW\r\nB W\r\nBBW", cube.Faces[4].PrintSide());
            Assert.Equal($"YYB\r\nY B\r\nYYB", cube.Faces[5].PrintSide());
        }

        [Fact]
        public void TestCubeTurnF()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.F();

            //Assert
            Assert.Equal($"WWW\r\nW W\r\nOOO", cube.Faces[0].PrintSide());
            Assert.Equal($"GGG\r\nG G\r\nGGG", cube.Faces[1].PrintSide());
            Assert.Equal($"OOY\r\nO Y\r\nOOY", cube.Faces[2].PrintSide());
            Assert.Equal($"WRR\r\nW R\r\nWRR", cube.Faces[3].PrintSide());
            Assert.Equal($"BBB\r\nB B\r\nBBB", cube.Faces[4].PrintSide());
            Assert.Equal($"YYY\r\nY Y\r\nRRR", cube.Faces[5].PrintSide());
        }

        [Fact]
        public void TestCubeTurnFPrime()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.FPrime();

            //Assert
            Assert.Equal($"WWW\r\nW W\r\nRRR", cube.Faces[0].PrintSide());
            Assert.Equal($"GGG\r\nG G\r\nGGG", cube.Faces[1].PrintSide());
            Assert.Equal($"OOW\r\nO W\r\nOOW", cube.Faces[2].PrintSide());
            Assert.Equal($"YRR\r\nY R\r\nYRR", cube.Faces[3].PrintSide());
            Assert.Equal($"BBB\r\nB B\r\nBBB", cube.Faces[4].PrintSide());
            Assert.Equal($"YYY\r\nY Y\r\nOOO", cube.Faces[5].PrintSide());
        }

        [Fact]
        public void TestCubeTurnD()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.D();

            //Assert
            Assert.Equal($"WWW\r\nW W\r\nWWW", cube.Faces[0].PrintSide());
            Assert.Equal($"GGG\r\nG G\r\nOOO", cube.Faces[1].PrintSide());
            Assert.Equal($"OOO\r\nO O\r\nBBB", cube.Faces[2].PrintSide());
            Assert.Equal($"RRR\r\nR R\r\nGGG", cube.Faces[3].PrintSide());
            Assert.Equal($"BBB\r\nB B\r\nRRR", cube.Faces[4].PrintSide());
            Assert.Equal($"YYY\r\nY Y\r\nYYY", cube.Faces[5].PrintSide());
        }

        [Fact]
        public void TestCubeTurnDPrime()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.DPrime();

            //Assert
            Assert.Equal($"WWW\r\nW W\r\nWWW", cube.Faces[0].PrintSide());
            Assert.Equal($"GGG\r\nG G\r\nRRR", cube.Faces[1].PrintSide());
            Assert.Equal($"OOO\r\nO O\r\nGGG", cube.Faces[2].PrintSide());
            Assert.Equal($"RRR\r\nR R\r\nBBB", cube.Faces[3].PrintSide());
            Assert.Equal($"BBB\r\nB B\r\nOOO", cube.Faces[4].PrintSide());
            Assert.Equal($"YYY\r\nY Y\r\nYYY", cube.Faces[5].PrintSide());
        }

        [Fact]
        public void TestCubeTurnB()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.B();

            //Assert
            Assert.Equal($"RRR\r\nW W\r\nWWW", cube.Faces[0].PrintSide());
            Assert.Equal($"GGG\r\nG G\r\nGGG", cube.Faces[1].PrintSide());
            Assert.Equal($"WOO\r\nW O\r\nWOO", cube.Faces[2].PrintSide());
            Assert.Equal($"RRY\r\nR Y\r\nRRY", cube.Faces[3].PrintSide());
            Assert.Equal($"BBB\r\nB B\r\nBBB", cube.Faces[4].PrintSide());
            Assert.Equal($"OOO\r\nY Y\r\nYYY", cube.Faces[5].PrintSide());
        }

        [Fact]
        public void TestCubeTurnBPrime()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.BPrime();

            //Assert
            Assert.Equal($"OOO\r\nW W\r\nWWW", cube.Faces[0].PrintSide());
            Assert.Equal($"GGG\r\nG G\r\nGGG", cube.Faces[1].PrintSide());
            Assert.Equal($"YOO\r\nY O\r\nYOO", cube.Faces[2].PrintSide());
            Assert.Equal($"RRW\r\nR W\r\nRRW", cube.Faces[3].PrintSide());
            Assert.Equal($"BBB\r\nB B\r\nBBB", cube.Faces[4].PrintSide());
            Assert.Equal($"RRR\r\nY Y\r\nYYY", cube.Faces[5].PrintSide());
        }
    }
}
