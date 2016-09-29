﻿/* 
 * Capture Test NyARToolkitCSサンプルプログラム
 * --------------------------------------------------------------------------------
 * The MIT License
 * Copyright (c) 2008 nyatla
 * airmail(at)ebony.plala.or.jp
 * http://nyatla.jp/nyartoolkit/
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * 
 */
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SimpleLiteDirect3d.WindowsMobile5
{


    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [MTAThread]
        static void Main()
        {

            // フォームとメインサンプルクラスを作成
            using (NyARToolkitCS frm = new NyARToolkitCS())
            {
                ResourceBuilder nyar_res;
                try
                {
                    nyar_res = new ResourceBuilder();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "SimpleLiteD3d::デバイスの初期化に失敗しました。");
                    return;
                }
                
                using (SimpleLiteD3d sample = new SimpleLiteD3d(frm,nyar_res))
                {


                    // メインフォームを表示
                    frm.Show();
                    //キャプチャ開始
                    sample.start();
                    Stopwatch sw = new Stopwatch();
                    // フォームにフォーカスがある間はループし続ける
                    while (frm.Focused)
                    {
                        sw.Start();
                        // メインループ処理を行う
                        sample.MainLoop();
                        //スレッドスイッチ
                        Thread.Sleep(0);



                        // イベントがある場合はその処理する
                        Application.DoEvents();
                        sw.Stop();
                        //sample.fps_x_100 = (int)(1000 * 100 / (sw.ElapsedMilliseconds+1));
                        sw.Reset();

                    }
                    //キャプチャの停止
                    //sample.stop();
                }
            }
            return;
        }
    }
}