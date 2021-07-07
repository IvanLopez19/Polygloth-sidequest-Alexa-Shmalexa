using System;
using System.Collections.Generic;

//Enumeradores de indicadores, lineas, modos, parametros y tamaños
enum indicators {NOTHING};
enum lines {NOLINE,SINGLELINE,DOUBLELINE};
enum modes {NOMODE,INT_FLOAT,INT_FLOAT_FLOAT_FLOAT,INT_INT_INT_INT_INT};
enum parametros {THERMAL_CONDUCTIVITY,HEAT_SOURCE};
enum tamanos {NODES,ELEMENTS,DIRICHLET,NEUMANN};

//Creacion de clase item con el objetivo de poder
unsafe abstract class item{

    //atributos de la clase item
    protected int id;
    //coordenadas de un nodo
    protected float x;
    protected float y;
    protected float z;
    //nodos de un elemento
    protected int node1;
    protected int node2;
    protected int node3;
    protected int node4;
    protected int node5;
    protected int node6;
    protected int node7;
    protected int node8;
    protected int node9;
    protected int node10;
    //valor de una condicion
    protected float value;

    //Getters y Setters
    public int getid(){
        return id;
    }

    public void setid(int id){
        this.id=id;
    }
    public float getx(){
        return x;
    }
    public void setx(float x){
        this.x=x;
    }
    public float gety(){
        return x;
    }    
    public void sety(float y){
        this.y=y;
    }
    public float getz(){
        return z;
    }
    public void setz(float z){
        this.z=z;
    }
    public int getNode1(){
        return node1;
    }
    public void setNode1(int node){
        this.node1=node;
    } 
    public int getNode2(){
        return node2;
    }
    public void setNode2(int node){
        this.node2=node;
    }
    public int getNode3(){
        return node3;
    }
    public void setNode3(int node){
        this.node3=node;
    }
    public int getNode4(){
        return node4;
    }
    public void setNode4(int node){
        this.node4=node;
    }
    public int getNode5(){
        return node5;
    }
    public void setNode5(int node){
        this.node5=node;
    }
    public int getNode6(){
        return node6;
    }
    public void setNode6(int node){
        this.node6=node;
    }
    public int getNode7(){
        return node7;
    }
    public void setNode7(int node){
        this.node7=node;
    }
    public int getNode8(){
        return node8;
    }
    public void setNode8(int node){
        this.node8=node;
    }
    public int getNode9(){
        return node9;
    }
    public void setNode9(int node){
        this.node8=node;
    }
    public int getNode10(){
        return node10;
    }
    public void setNode10(int node){
        this.node10=node;
    }
    public float getValue(){
        return value;
    }
    public void setValue(float value){
        this.value=value;
    }        
    public abstract void SetValues(int a,float b,float c,float d,int e,int f,int g, int h, int i, int j, int k, int m, int n, int o ,float p);
    
}

//Clase que representara un nodo de la malla, hereda de la clase item
class node: item{
    public override void SetValues(int a,float b,float c,float d,int e,int f,int g, int h, int i, int j, int k, int m, int n, int o, float p ){
        id = a;
        x = b;
        y = c;
        z = d;
    }
};

//Clase element que representa un elemento de la malla, hereda de la clase item
class element: item{
    public override void SetValues(int a,float b,float c,float d,int e,int f,int g, int h, int i, int j, int k, int m, int n, int o, float p){
        id = a;
        node1 = e;
        node2 = f;
        node3 = g;
        node4 = h;
        node5 = i;
        node6 = j;
        node7 = k;
        node8 = m;
        node9 = n;
        node10 = o;
    }

};
//Clase condition que representa una condicion de Dirichlet o de Neumman
class condition: item{
    public override void SetValues(int a,float b,float c,float d,int e,int f,int g, int h, int i, int j, int k, int m, int n, int o, float p){
        node1 = e;
        value = p;
    }

};

//Clase que representa la malla completa
class mesh{
    //Arreglos los valores de los parametros y tambien el tamaño de los arreglos de nodos, elementos, indices de Dirichlet,etc..
    float[] parameters = new float[2];
    int[] sizes = new int[4];
    
    //arreglos que guardaran los nodos, elementos,etc
    node[] nodos;
    element[] elementos;
    int[] indicesDirichlet;
    condition[] condicionesDirichlet;
    condition[] condicionesNeumann;

    //Getters y Setters de sizes y parameters
        public void setParameters(float k,float Q){
            parameters[((int)parametros.THERMAL_CONDUCTIVITY)]=k;
            parameters[((int)parametros.HEAT_SOURCE)]=Q;
        }
        public void setSizes(int nnodes,int neltos,int ndirich,int nneu){
            sizes[((int)tamanos.NODES)] = nnodes;
            sizes[((int)tamanos.ELEMENTS)] = neltos;
            sizes[((int)tamanos.DIRICHLET)] = ndirich;
            sizes[((int)tamanos.NEUMANN)] = nneu;
        }
        public int getSize(int s){
            return sizes[s];
        }
        public float getParameter(int p){
            return parameters[p];
        }

        //metodo que inicializa los arreglos con los tamaños contenidos en el arreglo de sizes
        public void createData(){
            nodos = new node[((int)tamanos.NODES)];
            elementos = new element[((int)tamanos.ELEMENTS)];
            indicesDirichlet = new int[((int)tamanos.DIRICHLET)];
            condicionesDirichlet = new condition[sizes[((int)tamanos.DIRICHLET)]];
            condicionesNeumann = new condition[sizes[((int)tamanos.NEUMANN)]];
        }

        //Getters y Setters de los arreglos de nodos, elementos, indices y condiciones
        public node[] getNodes(){
            return nodos;
        }
        public element[] getElements(){
            return elementos;
        }
        public int[] getDirichletIndices(){
            return indicesDirichlet;
        }
        public condition[] getDirichlet(){
            return condicionesDirichlet;
        }
        public condition[] getNeumann(){
            return condicionesNeumann;
        }
        node getNode(int i){
            return nodos[i];
        }
        element getElement(int i){
            return elementos[i];
        }
        condition getCondition(int i, int type){
            if(type == ((int)tamanos.DIRICHLET)) return condicionesDirichlet[i];
            else return condicionesNeumann[i];
        }
};
