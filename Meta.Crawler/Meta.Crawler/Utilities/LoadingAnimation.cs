using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta.Crawler.Utilities;

internal class LoadingAnimation
{
    private readonly char[] _sequence = { '/', '-', '\\', '|' };
    private int _counter;
    private bool _active;
    private Thread _thread;

    public LoadingAnimation()
    {
        _counter = 0;
        _active = false;
        _thread = new Thread(Spin);
    }

    public void Start()
    {
        if (_active) return;

        _active = true;
        _thread.Start();
    }

    public void Stop()
    {
        _active = false;
        _thread.Join();
        Console.Write("\b");
    }

    private void Spin()
    {
        while (_active)
        {
            Console.Write(_sequence[_counter % _sequence.Length]);
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            _counter++;
            Thread.Sleep(100);
        }
    }
}
