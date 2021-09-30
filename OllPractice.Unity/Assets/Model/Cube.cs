using System.Linq;

namespace Model
{
    public class Cube
    {
        private Face[] _faces;

        public Cube()
        {
            _faces = new Face[6];
            var centers = new RubikColor[6];
            //Init solved cube
            //Top face
            _faces[0] = new Face(RubikColor.W);
            centers[0] = RubikColor.W;
            //Right Face
            _faces[1] = new Face(RubikColor.R);
            centers[1] = RubikColor.R;
            //Front face
            _faces[2] = new Face(RubikColor.G);
            centers[2] = RubikColor.G;
            //Left face
            _faces[3] = new Face(RubikColor.O);
            centers[3] = RubikColor.O;
            //Back face
            _faces[4] = new Face(RubikColor.B);
            centers[4] = RubikColor.B;
            //Bottom face
            _faces[5] = new Face(RubikColor.Y);
            centers[5] = RubikColor.Y;
        }

        public Face[] GetFaces => _faces;

        public void U()
        {
            _faces[0].Rotate(Rotation.One);
            var greenSideBytes = _faces[2].RotateSide(0);
            var orangeSideBytes = _faces[3].RotateSide(0, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(0, orangeSideBytes);
            var redSideBytes = _faces[1].RotateSide(0, blueSideBytes);
            _faces[2].RotateSide(0, redSideBytes);
        }

        public void UPrime()
        {
            _faces[0].Rotate(Rotation.Prime);
            var greenSideBytes = _faces[2].RotateSide(0);
            var redSideBytes = _faces[1].RotateSide(0, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(0, redSideBytes);
            var orangeSideBytes = _faces[3].RotateSide(0, blueSideBytes);
            _faces[2].RotateSide(0, orangeSideBytes);
        }

        public void R()
        {
            _faces[1].Rotate(Rotation.One);
            var greenSideBytes = _faces[2].RotateSide(2);
            var whiteSideBytes = _faces[0].RotateSide(2, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(6, whiteSideBytes);
            var yellowSideBytes = _faces[5].RotateSide(2, blueSideBytes);
            _faces[2].RotateSide(2, yellowSideBytes);
        }
        public void RPrime()
        {
            _faces[1].Rotate(Rotation.Prime);
            var greenSideBytes = _faces[2].RotateSide(2);
            var yellowSideBytes = _faces[5].RotateSide(2, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(6, yellowSideBytes);
            var whiteSideBytes = _faces[0].RotateSide(2, blueSideBytes);
            _faces[2].RotateSide(2, whiteSideBytes);
        }
        public void L()
        {
            _faces[3].Rotate(Rotation.One);
            var greenSideBytes = _faces[2].RotateSide(6);
            var yellowSideBytes = _faces[5].RotateSide(6, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(2, yellowSideBytes);
            var whiteSideBytes = _faces[0].RotateSide(6, blueSideBytes);
            _faces[2].RotateSide(6, whiteSideBytes);

        }
        public void LPrime()
        {
            _faces[3].Rotate(Rotation.Prime);
            var greenSideBytes = _faces[2].RotateSide(6);
            var whiteSideBytes = _faces[0].RotateSide(6, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(2, whiteSideBytes);
            var yellowSideBytes = _faces[5].RotateSide(6, blueSideBytes);
            _faces[2].RotateSide(6, yellowSideBytes);

        }
        public void F()
        {
            _faces[2].Rotate(Rotation.One);
            var whiteSideBytes = _faces[0].RotateSide(4);
            var redSideBytes = _faces[1].RotateSide(6, whiteSideBytes);
            var yellowSideBytes = _faces[5].RotateSide(0, redSideBytes);
            var orangeSideBytes = _faces[3].RotateSide(2, yellowSideBytes);
            _faces[0].RotateSide(4, orangeSideBytes);

        }
        public void FPrime()
        {
            _faces[2].Rotate(Rotation.Prime);
            var whiteSideBytes = _faces[0].RotateSide(4);
            var orangeSideBytes = _faces[3].RotateSide(2, whiteSideBytes);
            var yellowSideBytes = _faces[5].RotateSide(0, orangeSideBytes);
            var redSideBytes = _faces[1].RotateSide(6, yellowSideBytes);
            _faces[0].RotateSide(4, redSideBytes);

        }
        public void D()
        {
            _faces[5].Rotate(Rotation.One);
            var greenSideBytes = _faces[2].RotateSide(4);
            var redSideBytes = _faces[1].RotateSide(4, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(4, redSideBytes);
            var orangeSideBytes = _faces[3].RotateSide(4, blueSideBytes);
            _faces[2].RotateSide(4, orangeSideBytes);


        }
        public void DPrime()
        {
            _faces[5].Rotate(Rotation.Prime);
            var greenSideBytes = _faces[2].RotateSide(4);
            var orangeSideBytes = _faces[3].RotateSide(4, greenSideBytes);
            var blueSideBytes = _faces[4].RotateSide(4, orangeSideBytes);
            var redSideBytes = _faces[1].RotateSide(4, blueSideBytes);
            _faces[2].RotateSide(4, redSideBytes);

        }
        public void B()
        {
            _faces[4].Rotate(Rotation.One);
            var whiteSideBytes = _faces[0].RotateSide(0);
            var orangeSideBytes = _faces[3].RotateSide(6, whiteSideBytes);
            var yellowSideBytes = _faces[5].RotateSide(4, orangeSideBytes);
            var redSideBytes = _faces[1].RotateSide(2, yellowSideBytes);
            _faces[0].RotateSide(0, redSideBytes);


        }
        public void BPrime()
        {
            _faces[4].Rotate(Rotation.Prime);
            var whiteSideBytes = _faces[0].RotateSide(0);
            var redSideBytes = _faces[1].RotateSide(2, whiteSideBytes);
            var yellowSideBytes = _faces[5].RotateSide(4, redSideBytes);
            var orangeSideBytes = _faces[3].RotateSide(6, yellowSideBytes);
            _faces[0].RotateSide(0, orangeSideBytes);

        }

        public void LoadState(Face[] faces) => _faces = faces;

        public string PrintCubeState() => _faces.Aggregate("", (current, face) => current + face.PrintSide());
    }
}
