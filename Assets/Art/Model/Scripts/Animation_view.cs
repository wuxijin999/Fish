using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class Animation_view : MonoBehaviour
{

    private Animator anim;
    public Vector2 scrollPosition = Vector2.zero;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    float m_Y = 0f;
    float y { get { return m_Y += 45; } }
    Vector2 size = new Vector2(300, 40);
    void OnGUI()
    {
        m_Y = 0f;
        GUI.Box(new Rect(10, 10, 350, 300), "");
        scrollPosition = GUI.BeginScrollView(new Rect(20, 20, 350, 280), scrollPosition, new Rect(0, 0, 100, 975));

        if (GUI.Button(new Rect(0, y, size.x, size.y), "Idle"))
            anim.SetBool("Idle", true);
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Move"))
            anim.SetBool("Move", true);
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Attack_1"))
            anim.SetBool("Attack_1", true);
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Attack_2"))
            anim.SetBool("Attack_2", true);
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Attack_3"))
            anim.SetBool("Attack_3", true);
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Attack_4"))
            anim.SetBool("Attack_4", true);
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Skill_1"))
            anim.SetBool("Skill_1", true);
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Skill_2"))
            anim.SetBool("Skill_2", true);
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Skill_3"))
            anim.SetBool("Skill_3", true);
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Skill_4"))
            anim.SetBool("Skill_4", true);
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Skill_Special"))
            anim.SetBool("Skill_Special", true);
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Dead"))
            anim.SetBool("Dead", true);
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Dance"))
            anim.SetBool("Dance", true);
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Hurt"))
            anim.SetBool("Hurt", true);
        if (GUI.Button(new Rect(0, y, size.x, size.y), "HurtDown"))
            anim.SetBool("HurtDown", true);
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Stun"))
            anim.SetBool("Stun", true);

        GUI.EndScrollView();


        m_Y = 0f;
        GUI.Box(new Rect(510, 10, 350, 300), "");
        scrollPosition = GUI.BeginScrollView(new Rect(520, 20, 350, 280), scrollPosition, new Rect(0, 0, 100, 975));

        if (GUI.Button(new Rect(0, y, size.x, size.y), "Idle"))
            anim.Play("Idle");
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Move"))
            anim.Play("Move");
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Attack_1"))
            anim.Play("Attack_1");
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Attack_2"))
            anim.Play("Attack_2");
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Attack_3"))
            anim.Play("Attack_3");
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Attack_4"))
            anim.Play("Attack_4");
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Skill_1"))
            anim.Play("Skill_1");
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Skill_2"))
            anim.Play("Skill_2");
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Skill_3"))
            anim.Play("Skill_3");
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Skill_4"))
            anim.Play("Skill_4");
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Skill_Special"))
            anim.Play("Skill_Special");
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Dead"))
            anim.Play("Dead");
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Dance"))
            anim.Play("Dance");
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Hurt"))
            anim.Play("Hurt");
        if (GUI.Button(new Rect(0, y, size.x, size.y), "HurtDown"))
            anim.Play("HurtDown");
        if (GUI.Button(new Rect(0, y, size.x, size.y), "Stun"))
            anim.Play("Stun");

        GUI.EndScrollView();
    }
}
