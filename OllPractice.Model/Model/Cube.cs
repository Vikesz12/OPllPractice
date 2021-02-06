using Model;

namespace OllPractice.Model.Model
{
    public class Cube
    {
        public readonly Face[] Faces;
        public readonly RubikColor[] Centers;
        public Cube()
        {
            Faces = new Face[6];
            Centers = new RubikColor[6];
            //Init solved cube
            //Top face
            Faces[0] = new Face(RubikColor.W);
            Centers[0] = RubikColor.W;
            //Front face
            Faces[1] = new Face(RubikColor.G);
            Centers[1] = RubikColor.G;
            //Left face
            Faces[2] = new Face(RubikColor.O);
            Centers[2] = RubikColor.O;
            //Right Face
            Faces[3] = new Face(RubikColor.R);
            Centers[3] = RubikColor.R;
            //Back face
            Faces[4] = new Face(RubikColor.B);
            Centers[4] = RubikColor.B;
            //Bottom face
            Faces[5] = new Face(RubikColor.Y);
            Centers[5] = RubikColor.Y;
        }

        public void U()
        {
            Faces[0].Rotate(Rotation.ONE);
            var greenSideBytes = Faces[1].RotateSide(0);
            var orangeSideBytes = Faces[2].RotateSide(0, greenSideBytes);
            var blueSideBytes = Faces[4].RotateSide(0, orangeSideBytes);
            var redSideBytes = Faces[3].RotateSide(0, blueSideBytes);
            Faces[1].RotateSide(0, redSideBytes);
        }

        public void UPrime()
        {
            Faces[0].Rotate(Rotation.PRIME);
            var greenSideBytes = Faces[1].RotateSide(0);
            var redSideBytes = Faces[3].RotateSide(0, greenSideBytes);
            var blueSideBytes = Faces[4].RotateSide(0, redSideBytes);
            var orangeSideBytes = Faces[2].RotateSide(0, blueSideBytes);
            Faces[1].RotateSide(0, orangeSideBytes);
        }

        public void R()
        {
            Faces[3].Rotate(Rotation.ONE);
            var greenSideBytes = Faces[1].RotateSide(2);
            var whiteSideBytes = Faces[0].RotateSide(2, greenSideBytes);
            var blueSideBytes = Faces[4].RotateSide(6, whiteSideBytes);
            var yellowSideBytes = Faces[5].RotateSide(6, blueSideBytes);
            Faces[1].RotateSide(2, yellowSideBytes);
        }
        public void RPrime()
        {
            Faces[3].Rotate(Rotation.PRIME);
            var greenSideBytes = Faces[1].RotateSide(2);
            var yellowSideBytes = Faces[5].RotateSide(6, greenSideBytes);
            var blueSideBytes = Faces[4].RotateSide(6, yellowSideBytes);
            var whiteSideBytes = Faces[0].RotateSide(2, blueSideBytes);
            Faces[1].RotateSide(2, whiteSideBytes);
        }
        public void L()
        {
            Faces[2].Rotate(Rotation.ONE);
            var greenSideBytes = Faces[1].RotateSide(6);
            var yellowSideBytes = Faces[5].RotateSide(2, greenSideBytes);
            var blueSideBytes = Faces[4].RotateSide(2, yellowSideBytes);
            var whiteSideBytes = Faces[0].RotateSide(6, blueSideBytes);
            Faces[1].RotateSide(6, whiteSideBytes);

        }
        public void LPrime()
        {
            Faces[2].Rotate(Rotation.PRIME);
            var greenSideBytes = Faces[1].RotateSide(6);
            var whiteSideBytes = Faces[0].RotateSide(6, greenSideBytes);
            var blueSideBytes = Faces[4].RotateSide(2, whiteSideBytes);
            var yellowSideBytes = Faces[5].RotateSide(2, blueSideBytes);
            Faces[1].RotateSide(6, yellowSideBytes);

        }
        public void F()
        {
            Faces[1].Rotate(Rotation.ONE);
            var whiteSideBytes = Faces[0].RotateSide(4);
            var redSideBytes = Faces[3].RotateSide(6, whiteSideBytes);
            var yellowSideBytes = Faces[5].RotateSide(4, redSideBytes);
            var orangeSideBytes = Faces[2].RotateSide(2, yellowSideBytes);
            Faces[0].RotateSide(4, orangeSideBytes);

        }
        public void FPrime()
        {
            Faces[1].Rotate(Rotation.PRIME);
            var whiteSideBytes = Faces[0].RotateSide(4);
            var orangeSideBytes = Faces[2].RotateSide(2, whiteSideBytes);
            var yellowSideBytes = Faces[5].RotateSide(4, orangeSideBytes);
            var redSideBytes = Faces[3].RotateSide(6, yellowSideBytes);
            Faces[0].RotateSide(4, redSideBytes);

        }
        public void D()
        {
            Faces[5].Rotate(Rotation.ONE);
            var greenSideBytes = Faces[1].RotateSide(4);
            var redSideBytes = Faces[3].RotateSide(4, greenSideBytes);
            var blueSideBytes = Faces[4].RotateSide(4, redSideBytes);
            var orangeSideBytes = Faces[2].RotateSide(4, blueSideBytes);
            Faces[1].RotateSide(4, orangeSideBytes);


        }
        public void DPrime()
        {
            Faces[5].Rotate(Rotation.PRIME);
            var greenSideBytes = Faces[1].RotateSide(4);
            var orangeSideBytes = Faces[2].RotateSide(4, greenSideBytes);
            var blueSideBytes = Faces[4].RotateSide(4, orangeSideBytes);
            var redSideBytes = Faces[3].RotateSide(4, blueSideBytes);
            Faces[1].RotateSide(4, redSideBytes);

        }
        public void B()
        {
            Faces[4].Rotate(Rotation.ONE);
            var whiteSideBytes = Faces[0].RotateSide(0);
            var orangeSideBytes = Faces[2].RotateSide(6, whiteSideBytes);
            var yellowSideBytes = Faces[5].RotateSide(0, orangeSideBytes);
            var redSideBytes = Faces[3].RotateSide(2, yellowSideBytes);
            Faces[0].RotateSide(0, redSideBytes);


        }
        public void BPrime()
        {
            Faces[4].Rotate(Rotation.PRIME);
            var whiteSideBytes = Faces[0].RotateSide(0);
            var redSideBytes = Faces[3].RotateSide(2, whiteSideBytes);
            var yellowSideBytes = Faces[5].RotateSide(0, redSideBytes);
            var orangeSideBytes = Faces[2].RotateSide(6, yellowSideBytes);
            Faces[0].RotateSide(0, orangeSideBytes);

        }
    }
}
