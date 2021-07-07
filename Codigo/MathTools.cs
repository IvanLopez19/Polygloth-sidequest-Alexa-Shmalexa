using System;
using System.Collections.Generic;
using System.Linq;

//Clases en lugar de typedef
class Vector{
    public float[] vector;
    public Vector(int a){
        vector = new float[a];
    }
}

class Matrix{
    public float[,] matrix;
    public Matrix(int a, int b){
        matrix = new float[a,b];
    }
}

//crear clase obligatoriamente
class   MathTools{
static public void zeroes(Matrix M,int n){
    for(int i=0;i<n;i++){
        for(int j=0;j<n;j++){
            M.matrix[i,j]=0;
        }
    }
}
static void zeroes(Matrix M,int n,int m){
    for(int i=0;i<n;i++){
        for(int j=0;j<m;j++){
            M.matrix[i,j]=0;
        }
    }
}
static void zeroes(Vector v,int n){
    for(int i=0;i<n;i++){
        v.vector[i]=0;
    }
}
//No es exactamente matrix.Length
static void copyMatrix(Matrix A, Matrix copy){
    zeroes(copy,A.matrix.Length);
    for(int i=0;i<A.matrix.Length;i++)
        for(int j=0;j<A.matrix.Length;j++)
            copy.matrix[i,j] = A.matrix[i,j];
}
static float calculateMember(int i,int j,int r,Matrix A,Matrix B){
    float member = 0;
    for(int k=0;k<r;k++)
        member += A.matrix[i,j]*B.matrix[i,j];
    return member;
}
static Matrix productMatrixMatrix(Matrix A,Matrix B,int n,int r,int m){
    Matrix R = new Matrix(A.matrix.Length,B.matrix.Length);

    zeroes(R,n,m);
    for(int i=0;i<n;i++)
        for(int j=0;j<m;j++)
            R.matrix[i,j] = calculateMember(i,j,r,A,B);

    return R;
}
void productMatrixVector(Matrix A, Vector v, Vector R){
    for(int f=0;f<A.matrix.Length;f++){
        float cell = 0.0f;
        for(int c=0;c<v.vector.Length;c++){
            cell += A.matrix[f,c]*v.vector[c];
        }
        R.vector[f] += cell;
    }
}
void productRealMatrix(float real,Matrix M,Matrix R){
    zeroes(R,M.matrix.Length);
    for(int i=0;i<M.matrix.Length;i++)
        for(int j=0;j<M.matrix.Length;j++)
            R.matrix[i,j] = real*M.matrix[i,j];
}

void getMinor(Matrix M,int i, int j){
    //cout << "Calculando menor ("<<i+1<<","<<j+1<<")...\n";
    //M.erase(M.begin()+i);
    M.matrix = M.matrix.Where((source, index) =>index != indexToRemove).ToArray();
    for(int i=0;i<M.size();i++)
        M.at(i).erase(M.at(i).begin()+j);
}
float determinant(Matrix M){
    if(M.matrix.Length == 1) return M.matrix[0,0];
    else{
        float det=0.0f;
        for(int i=0;i<M.matrix[0,];i++){
            Matrix minor;
            copyMatrix(M,minor);
            getMinor(minor,0,i);
            det += pow(-1,i)*M.at(0).at(i)*determinant(minor);
        }
        return det;
    }
}
void cofactors(Matrix M, Matrix &Cof){
    zeroes(Cof,M.size());
    for(int i=0;i<M.size();i++){
        for(int j=0;j<M.at(0).size();j++){
            //cout << "Calculando cofactor ("<<i+1<<","<<j+1<<")...\n";
            Matrix minor;
            copyMatrix(M,minor);
            getMinor(minor,i,j);
            Cof.at(i).at(j) = pow(-1,i+j)*determinant(minor);
        }
    }
}

void transpose(Matrix M, Matrix &T){
    zeroes(T,M.at(0).size(),M.size());
    for(int i=0;i<M.size();i++)
        for(int j=0;j<M.at(0).size();j++)
            T.at(j).at(i) = M.at(i).at(j);
}

void inverseMatrix(Matrix M, Matrix &Minv){
    cout << "Iniciando calculo de inversa...\n";
    Matrix Cof, Adj;
    cout << "Calculo de determinante...\n";
    float det = determinant(M);
    if(det == 0) exit(EXIT_FAILURE);
    cout << "Iniciando calculo de cofactores...\n";
    cofactors(M,Cof);
    cout << "Calculo de adjunta...\n";
    transpose(Cof,Adj);
    cout << "Calculo de inversa...\n";
    productRealMatrix(1/det,Adj,Minv);
}

}