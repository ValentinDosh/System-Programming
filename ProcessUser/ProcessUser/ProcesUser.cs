using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessUser
{
    class ProccesUser
    {
        private IOrderedEnumerable<Process> _proceces;

        // текущий процесс(последний с которым ввзаимодействовали)
        private Process _currentProcessId;
        
        // узнать(получить) текущий процесс
        public Process CurrentProcess
        {
            get
            {
                return _currentProcessId;
            }
        }

        public ProccesUser() 
        {
            _proceces = null;
            _currentProcessId = null;
        }

        // список всех процессов
        public void DisplayProcceces()
        {
            _proceces = Process.GetProcesses(".").Select(proc => proc).OrderBy(proc => proc.Id);

            foreach (var item in _proceces)
            {
                Console.WriteLine("({0}) {1}", item.Id, item.ProcessName);
            }
        }

        // выбрать процесс по Id
        public bool IdProcess(int id)
        {
            if (_proceces == null)
            {
                _proceces = Process.GetProcesses(".").Select(proc => proc).OrderBy(proc => proc.Id);
            }
            try
            {
                foreach (var item in _proceces)
                {
                    if (id == item.Id)
                    {
                        _currentProcessId = item;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }



        // запустить выбранный процесс
        public void ProcessStart(string argumnent)
        {
            try
            {
                _currentProcessId = Process.Start(argumnent);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ProcessStart(string FileName, string argumnent)
        {
            try
            {
                _currentProcessId = Process.Start(FileName, argumnent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ProcessStart(ProcessStartInfo arg)
        {
            try
            {
                _currentProcessId = Process.Start(arg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        // остановить процесс используя Id
        public void Stop(int id)
        {
            if (IdProcess(id))
            {
                _currentProcessId.Kill();
                _currentProcessId = null;
            }

        }

        // остановить текущий процесс используя
        public void Stop()
        {
            _currentProcessId.Kill();
            _currentProcessId = null;
        }


        // информация по потокам(выодим инф текущего)
        public void DisplayThreads()
        {
            try
            {
                var threads = _currentProcessId.Threads;
                foreach (ProcessThread item in threads)
                {
                    Console.WriteLine("Id потока: {0}, время запуска потока: {1}", item.Id, item.StartTime);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // информация по модулям
        public void DisplayModules()
        {
            try
            {
                var modules = _currentProcessId.Modules;
                foreach (ProcessModule item in modules)
                {
                    Console.WriteLine("Имя модуля: {0}, Путь модуля: {1}", item.ModuleName, item.FileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
