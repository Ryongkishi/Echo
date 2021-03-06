﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//处理操作角色和同步角色的一些共有功能
public class BaseHuman : MonoBehaviour
{
    //是否正在移动
    protected bool isMoving = false;
    //移动目标点
    private Vector3 targetPosition;
    //移动速度
    public float speed = 1.2f;
    //动画组件
    private Animator animator;
    //描述
    public string desc ="";
    //移动到某处
    public void MoveTo(Vector3 pos){
        targetPosition = pos;
        isMoving = true;
        animator.SetBool("isMoving",true);
    }
    //移动updata
    public void MoveUpdate(){
        if(isMoving == false){
            return;
        }
        Vector3 pos = transform.position;
        transform.position = Vector3.MoveTowards(pos,targetPosition+new Vector3(0,1.0f,0),speed*Time.deltaTime);
        transform.LookAt(targetPosition+new Vector3(0,1.0f,0));
        if(Vector3.Distance(pos,targetPosition+new Vector3(0,1.0f,0))<0.05f){
            isMoving = false;
            animator.SetBool("isMoving",false);
        }
    }

    protected void Start()
    {
        animator = GetComponent<Animator>();

    }
    protected void Update()
    {
        MoveUpdate();
    }
}
