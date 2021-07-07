using System;
using System.IO;

//Debemos crear la clase Tools para agregar todos los metodos dentro de la clase
class Tools{

void obtenerDatos(ref StreamReader file,int nlines,int n,int mode,item[] item_list){
    file.ReadLine();
    //Si el numero de lineas es doubleline entonces se saltara otra linea
    if(nlines==((int)lines.DOUBLELINE)) file.ReadLine();

    for(int i=0;i<n;i++){
        switch(mode){
        case ((int)modes.INT_FLOAT):
            int e0; float r0;
            e0= file.Read();
            r0= file.Read();
            item_list[i].SetValues(((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),e0,((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),r0);
            break;
        case ((int)modes.INT_FLOAT_FLOAT_FLOAT):
            int e; float r,rr,rrr;
            e=file.Read();
            r=file.Read();
            rr=file.Read();
            rrr=file.Read();
            item_list[i].SetValues(e,r,rr,rrr,((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),((float)indicators.NOTHING));
            break;
        case ((int)modes.INT_INT_INT_INT_INT):
            int e1,e2,e3,e4,e5,e6,e7,e8,e9,e10,e11;
            e1=file.Read();
            e2=file.Read();
            e3=file.Read();
            e4=file.Read();
            e5=file.Read();
            e6=file.Read();
            e7=file.Read();
            e8=file.Read();
            e9=file.Read();
            e10=file.Read();
            e11=file.Read();
            item_list[i].SetValues(e1,((int)indicators.NOTHING),((int)indicators.NOTHING),((int)indicators.NOTHING),e2,e3,e4,e5,e6,e7,e8,e9,e10,e11,((float)indicators.NOTHING));
            break;
        }
    }
}

//Crear condiciones de dirichlet o neumman
void correctConditions(int n,ref condition[] list,ref int[] indices){
    for(int i=0;i<n;i++)
        indices[i] = list[i].getNode1();

    for(int i=0;i<n-1;i++){
        int pivot = list[i].getNode1();
        for(int j=i;j<n;j++)
            //Si la condición actual corresponde a un nodo posterior al nodo eliminado por
            //aplicar la condición anterior, se debe actualizar su posición.
            if(list[j].getNode1()>pivot)
                list[j].setNode1(list[j].getNode1()-1);
    }
}
//Añadir extenciones a archivos
unsafe void addExtension(ref string newfilename,ref string filename, ref string extension){
    newfilename = filename+extension+'\0';
}
unsafe void leerMallayCondiciones(ref mesh m,ref string filename){
    string inputfilename="";
    float k,Q;
    int nnodes,neltos,ndirich,nneu;
    string type = ".dat";

    addExtension(ref inputfilename,ref filename,ref type);
     StreamReader file = new StreamReader(inputfilename);

    k=file.Read();
    Q=file.Read();

    nnodes=file.Read();
    neltos=file.Read(); 
    ndirich=file.Read();
    nneu=file.Read();


    m.setParameters(k,Q);
    m.setSizes(nnodes,neltos,ndirich,nneu);
    m.createData();

    obtenerDatos(ref file,((int)lines.SINGLELINE),nnodes,((int)modes.INT_FLOAT_FLOAT_FLOAT),m.getNodes());
    obtenerDatos(ref file,((int)lines.DOUBLELINE),neltos,((int)modes.INT_INT_INT_INT_INT),m.getElements());
    obtenerDatos(ref file,((int)lines.DOUBLELINE),ndirich,((int)modes.INT_FLOAT),m.getDirichlet());
    obtenerDatos(ref file,((int)lines.DOUBLELINE),nneu,((int)modes.INT_FLOAT),m.getNeumann());


    condition[] conditions = m.getDirichlet();
    int[] indices = m.getDirichletIndices();

    correctConditions(ndirich,ref conditions,ref indices);

    }
    bool findIndex(int v, int s, ref int[] arr){
    for(int i=0;i<s;i++)
        if(arr[i]==v) return true;
    return false;
}

void writeResults(mesh m,Vector T,ref char[] filename){
    char[] outputfilename = new char[150];
    string a = outputfilename.ToString();
    int[] dirich_indices = m.getDirichletIndices();
    condition[] dirich = m.getDirichlet();
    string b = filename.ToString();
    string c = ".post.res";

    addExtension(ref a, ref b,ref c);
    StreamWriter file= new StreamWriter(a);

    file.WriteLine("GiD Post Results File 1.0\n");
    file.WriteLine("Result \"Temperature\" \"Load Case 1\" 1 Scalar OnNodes\nComponentNames \"T\"\nValues\n");

    int Tpos = 0;
    int Dpos = 0;
    int n = m.getSize(((int)tamanos.NODES));
    int nd = m.getSize(((int)tamanos.DIRICHLET));
    for(int i=0;i<n;i++){
        if(findIndex(i+1,nd,ref dirich_indices)){
            file.Write(i+1);
            file.Write(" ");
            file.Write(dirich[Dpos].getValue());
            file.Write("\n");
            Dpos++;
        }else{
            file.Write(i+1);
            file.Write(" "); 
            file.Write(T.vector[i]);
            file.Write("\n");
            Tpos++;
        }
    }

    file.WriteLine("End values\n");

}
}