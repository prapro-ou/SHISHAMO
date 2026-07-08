#include<stdio.h>
int main(void){
    int a;
    scanf("%d",&a);
    if(a<-10){
        printf("range1\n");
    }else if(-10<=a && a<0){
        printf("range2\n");
    }else{
            printf("range3\n");
        }
}
