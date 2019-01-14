using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHalo : MonoBehaviour {
    private ParticleSystem particleSys;  
    private ParticleSystem.Particle[] particleArr;  
    private CirclePosition[] circle; 
    public Gradient colorGradient;

    public int count = 10000;        
    public float size = 0.01f;      
    public float minRadius = 8.0f;   
    public float maxRadius = 12.0f; 
    public bool clockwise = true;   
    public float speed = 2f;        
    public float pingPong = 0.02f; 
    private int tier = 10;  
    // Use this for initialization
    void Start()
    {   // 初始化粒子数组  
        particleArr = new ParticleSystem.Particle[count];
        circle = new CirclePosition[count];
        
        particleSys = this.GetComponent<ParticleSystem>();
        particleSys.startSpeed = 0;          
        particleSys.startSize = size;           
        particleSys.loop = false;
        particleSys.maxParticles = count;      
        particleSys.Emit(count);               
        particleSys.GetParticles(particleArr);

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[5];
        alphaKeys[0].time = 0.0f; alphaKeys[0].alpha = 1.0f;
        alphaKeys[1].time = 0.4f; alphaKeys[1].alpha = 0.4f;
        alphaKeys[2].time = 0.6f; alphaKeys[2].alpha = 1.0f;
        alphaKeys[3].time = 0.9f; alphaKeys[3].alpha = 0.4f;
        alphaKeys[4].time = 1.0f; alphaKeys[4].alpha = 0.9f;
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].time = 0.0f; colorKeys[0].color = Color.white;
        colorKeys[1].time = 1.0f; colorKeys[1].color = Color.white;
        colorGradient.SetKeys(colorKeys, alphaKeys);

        for (int i = 0; i < count; ++i)
        {
            float midRadius = (maxRadius + minRadius) / 2;
            float minRate = Random.Range(1.0f, midRadius / minRadius);
            float maxRate = Random.Range(midRadius / maxRadius, 1.0f);
            float radius = Random.Range(minRadius * minRate, maxRadius * maxRate);
            float angle = Random.Range(0.0f, 360.0f);
            float theta = angle / 180 * Mathf.PI;
            float time = Random.Range(0.0f, 360.0f);

            circle[i] = new CirclePosition(radius, angle, time);

            particleArr[i].position = new Vector3(circle[i].radius * Mathf.Cos(theta), 0f, circle[i].radius * Mathf.Sin(theta));
        }

        particleSys.SetParticles(particleArr, particleArr.Length);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < count; i++)
        {
            if (clockwise)
                circle[i].angle -= (i % tier + 1) * (speed / circle[i].radius / tier);
            else 
                circle[i].angle += (i % tier + 1) * (speed / circle[i].radius / tier);
            
            circle[i].angle = (360.0f + circle[i].angle) % 360.0f;
            float theta = circle[i].angle / 180 * Mathf.PI;

            particleArr[i].position = new Vector3(circle[i].radius * Mathf.Cos(theta), 0f, circle[i].radius * Mathf.Sin(theta));
            
            circle[i].time += Time.deltaTime;
            circle[i].radius += Mathf.PingPong(circle[i].time / minRadius / maxRadius, pingPong) - pingPong / 2.0f;

            particleArr[i].startColor = colorGradient.Evaluate(circle[i].angle / 360.0f);  
        }

        particleSys.SetParticles(particleArr, particleArr.Length);
    }
}
