using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour {
	public GUISkin Player1;
	public GUISkin Player2;
	public GUISkin Customer;
	public string message = "轮到甲方";
	public string string1 = "";
	public string string2 = "";
	public string string3 = "";
	public string string4 = "";
	public string string5 = "";
	public string string6 = "";
	public string string7 = "";
	public string string8 = "";
	public string string9 = "";
	int[] player1 = new int[10]{0,0,0,0,0,0,0,0,0,0} ;
	int[] player2 = new int[10]{0,0,0,0,0,0,0,0,0,0} ;
	int count1 = 0;
	int count2 = 0;
	bool gameover = false;
	int[] selected = new int[10]{0,0,0,0,0,0,0,0,0,0};

	//public GUISkin mySkin;
	bool Adjust(int[] player){
		if (player [1] == 1 && player [2] == 1 && player [3] == 1)
			return true;
		if (player [4] == 1 && player [5] == 1 && player [6] == 1)
			return true;
		if (player [7] == 1 && player [8] == 1 && player [9] == 1)
			return true;
		if (player [1] == 1 && player [4] == 1 && player [7] == 1)
			return true;
		if (player [2] == 1 && player [5] == 1 && player [8] == 1)
			return true;
		if (player [3] == 1 && player [6] == 1 && player [9] == 1)
			return true;
		if (player [1] == 1 && player [5] == 1 && player [9] == 1)
			return true;
		if (player [3] == 1 && player [5] == 1 && player [7] == 1)
			return true;
		return false;

	}

	void Start(){
		Debug.Log ("游戏开始!\n");
	}
	void Update(){
		Debug.Log ("游戏正在进行中!\n");

	}


	void OnGUI () {
		GUI.skin = Customer;
		GUI.BeginGroup (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 210, 300, 420));
		if (GUI.Button (new Rect (0, 300, 300, 80), message)) {
			string temp = message;
			message = temp + "\n(玩游戏要专心,不要乱点啊！)";
		}
		if(GUI.Button (new Rect (0, 380, 100, 30), "帮助")) {
			string temp = message;
			message = temp + "\n(还没有写这个，请自己百度！)";
		}

		if(GUI.Button (new Rect (200, 380, 100, 30), "退出")) {
			Application.Quit ();
		}
		if(GUI.Button (new Rect (100, 380, 100, 30), "重来")) {
			string1 = "";
			string2 = "";
			string3 = "";
			string4 = "";
			string5 = "";
			string6 = "";
			string7 = "";
			string8 = "";
			string9 = "";
			message = "轮到甲方";
			count1 = 0;
			count2 = 0;
			gameover = false;
			for (int i = 0; i < 10; i++) {
				player1 [i] = 0;
				player2 [i] = 0;
				selected [i] = 0;
			}
		}
		GUI.Box (new Rect (0,0,300,300), "井字棋");


		if (GUI.Button (new Rect (20,20,80,80), string1)) {
			if (gameover)
				return;
			if (selected [1] == 0) {
				if (count1 <= count2) {
					string1 = "甲方";
					player1 [1] = 1;
					count1++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到乙方\n";
				}
				else {
					string1 = "乙方";
					player2 [1] = 1;
					count2++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到甲方\n";
				}

				selected [1] = 1;

				if (Adjust (player1)) {
					message = "游戏结束：\n甲方获胜！";
					GUI.backgroundColor = Color.red;gameover = true;
				}
				if (Adjust (player2)) {
					message = "游戏结束：\n乙方获胜！";gameover = true;
					GUI.backgroundColor = Color.red;
				}

			} else {
				message =  "操作失败：\n这个地方已经有棋子了！";
			}


			// This code is executed when the Button is clicked
		}
		if (GUI.Button (new Rect (20,115,80,80),string4)) {
			if (gameover)
				return;
			if (selected [4] == 0) {
				if (count1 <= count2) {
					string4 = "甲方";
					player1 [4] = 1;
					count1++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到乙方\n";
				}
				else {
					string4 = "乙方";
					player2 [4] = 1;
					count2++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到甲方\n";
				}
				selected [4] = 1;

				if (Adjust (player1)) {
					message = "游戏结束：\n甲方获胜！";
					GUI.backgroundColor = Color.red;gameover = true;
				}
				if (Adjust (player2)) {
					message = "游戏结束：\n乙方获胜！";
					GUI.backgroundColor = Color.red;gameover = true;
				}

			} else {
				message =  "操作失败：\n这个地方已经有棋子了！";
			}
		}
		if (GUI.Button (new Rect (20,210,80,80),  	string7)) {
			if (gameover)
				return;
			if (selected [7] == 0) {
				if (count1 <= count2) {
					string7 = "甲方";
					player1 [7] = 1;
					count1++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到乙方\n";
				}
				else {
					string7 = "乙方";
					player2 [7] = 1;
					count2++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到甲方\n";
				}
				selected [7] = 1;

				if (Adjust (player1)) {
					message = "游戏结束：\n甲方获胜！";gameover = true;
					GUI.backgroundColor = Color.red;
				}
				if (Adjust (player2)) {
					message = "游戏结束：\n乙方获胜！";gameover = true;
					GUI.backgroundColor = Color.red;
				}

			} else {
				message =  "操作失败：\n这个地方已经有棋子了！";
			}
		}

		//2 column

		if (GUI.Button (new Rect (110,20,80,80),  string2)) {
			if (gameover)
				return;
			if (selected [2] == 0) {
				if (count1 <= count2) {
					string2 = "甲方";
					player1 [2] = 1;
					count1++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到乙方\n";
				}
				else {
					string2 = "乙方";
					player2 [2] = 1;
					count2++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到甲方\n";
				}
				selected [2] = 1;

				if (Adjust (player1)) {
					message = "游戏结束：\n甲方获胜！";gameover = true;
					GUI.backgroundColor = Color.red;
				}
				if (Adjust (player2)) {
					message = "游戏结束：\n乙方获胜！";
					GUI.backgroundColor = Color.red;gameover = true;

				}

			} else {
				message =  "操作失败：\n这个地方已经有棋子了！";
			}
		}
		if (GUI.Button (new Rect (110,115,80,80),  string5)) {
			if (gameover)
				return;
			if (selected [5] == 0) {
				if (count1 <= count2) {
					string5 = "甲方";
					player1 [5] = 1;
					count1++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到乙方\n";
				}
				else {
					string5 = "乙方";
					player2 [5] = 1;
					count2++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到甲方\n";
				}
				selected [5] = 1;

				if (Adjust (player1)) {
					message = "游戏结束：\n甲方获胜！";gameover = true;
					GUI.backgroundColor = Color.red;
				}
				if (Adjust (player2)) {
					message = "游戏结束：\n乙方获胜！";gameover = true;
					GUI.backgroundColor = Color.red;

				}

			} else {
				message =  "操作失败：\n这个地方已经有棋子了！";
			}
		}
		if (GUI.Button (new Rect (110,210,80,80),  string8)) {
			if (gameover)
				return;
			if (selected [8] == 0) {
				if (count1 <= count2) {
					string8 = "甲方";
					player1 [8] = 1;
					count1++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到乙方\n";
				}
				else {
					string8 = "乙方";
					player2 [8] = 1;
					count2++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到甲方\n";
				}
				selected [8] = 1;

				if (Adjust (player1)) {
					message = "游戏结束：\n甲方获胜！";gameover = true;
					GUI.backgroundColor = Color.red;
				}
				if (Adjust (player2)) {
					message = "游戏结束：\n乙方获胜！";gameover = true;
					GUI.backgroundColor = Color.red;

				}

			} else {
				message =  "操作失败：\n这个地方已经有棋子了！";
			}
		}


		//3column
		if (GUI.Button (new Rect (200,20,80,80), string3)) {
			if (gameover)
				return;
			if (selected [3] == 0) {
				if (count1 <= count2) {
					string3 = "甲方";
					player1 [3] = 1;
					count1++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到乙方\n";
				}
				else {
					string3 = "乙方";
					player2 [3] = 1;
					count2++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到甲方\n";
				}
				selected [3] = 1;

				if (Adjust (player1)) {
					message = "游戏结束：\n甲方获胜！";GUI.backgroundColor = Color.red;	gameover = true;			}
				if (Adjust (player2)) {
					message = "游戏结束：\n乙方获胜！";GUI.backgroundColor = Color.red;gameover = true;
				}

			} else {
				message =  "操作失败：\n这个地方已经有棋子了！";
			}
		}
		if (GUI.Button (new Rect (200,115,80,80),   string6)) {
			if (gameover)
				return;
			if (selected [6] == 0) {
				if (count1 <= count2) {
					string6 = "甲方";
					player1 [6] = 1;
					count1++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到乙方\n";
				}
				else {
					string6 = "乙方";
					player2 [6] = 1;
					count2++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";gameover = true;
					}
					else
						message = "轮到甲方\n";
				}
				selected [6] = 1;

				if (Adjust (player1)) {
					message = "游戏结束：\n甲方获胜！";GUI.backgroundColor = Color.red;
					gameover = true;
				}
				if (Adjust (player2)) {
					message = "游戏结束：\n乙方获胜！";GUI.backgroundColor = Color.red;
					gameover = true;
				}

			} else {
				message =  "操作失败：\n这个地方已经有棋子了！";
			}
		}
		if (GUI.Button (new Rect (200,210,80,80), string9)) {
			if (gameover)
				return;
			if (selected [9] == 0) {
				if (count1 <= count2) {
					string9 = "甲方";
					player1 [9] = 1;
					count1++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";
						gameover = true;
					}
					else
						message = "轮到乙方\n";
				}
				else {
					string9 = "乙方";
					player2 [9] = 1;
					count2++;
					if (count1 + count2 == 9 && !Adjust (player1) && !Adjust (player2)) {
						message = "游戏结束：和棋！";
						gameover = true;
					}
					else
						message = "轮到甲方\n";
				}
				selected [9] = 1;

				if (Adjust (player1)) {
					message = "游戏结束：\n甲方获胜！";GUI.backgroundColor = Color.red;
					gameover = true;
				}
				if (Adjust (player2)) {
					message = "游戏结束：\n乙方获胜！";GUI.backgroundColor = Color.red;
					gameover = true;
				}

			} else {
				message =  "操作失败：\n这个地方已经有棋子了！";
			}
		}

		// End the group we started above. This is very important to remember!
		GUI.EndGroup ();
	}
}




