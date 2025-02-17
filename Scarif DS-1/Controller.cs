using System;
using System.IO;
using Scarif_DS_1.exceptions;
using Scarif_DS_1.modulos;
using Scarif_DS_1.ui;

namespace Scarif_DS_1
{
    public class Controller
    {
        private Model modelo;
        private IView ui;
        private bool executar;
        private string conteudo;
        private int valor;

        //Construtor do Controlador
        internal Controller()
        {
            modelo = new Model();
            ui = new Consola(modelo, this);
        }

        public void IniciarPrograma()
        {
            ui.AtivarInterface();
            Executar = true;
            while(Executar){
                ui.DisponibilizarOpcoes();
            }
            ui.EncerrarPrograma();
        }

        //Função que permite atualizar os dados do modelo
        public void SubmeterDados(string texto, TipoDados tipo)
        {
            Conteudo = texto;
            ValidarDados(tipo);
        }
        
        public void SubmeterDados(int pagina, TipoDados tipo)
        {
            Valor = pagina;
            ValidarDados(tipo);
        }

        private void ValidarDados(TipoDados tipo)
        {
            int separar;
            try
            {
                switch (tipo)
                {
                    case TipoDados.CaminhoOrigem:
                        //Prepara os dados para fornecer ao Modelo                                                  
                        separar = Conteudo.LastIndexOf("/", StringComparison.Ordinal);                                               
                        modelo.PathOrigem = Conteudo.Substring(0, separar+1);                                  
                        modelo.FileOrigem = Conteudo.Substring(separar + 1);                                   
                        if (!File.Exists(Conteudo))                                                            
                        {                                                                                           
                            throw new ExceptionFileNotFound("Ficheiro não encontrado!" , Conteudo);            
                        }                                                                                           
                        break;
                    case TipoDados.CaminhoOrigem2:
                        //Prepara os dados para fornecer ao Modelo                                                   
                        separar = Conteudo.LastIndexOf("/", StringComparison.Ordinal);                                               
                        modelo.PathOrigem2 = Conteudo.Substring(0, separar+1);                                 
                        modelo.FileOrigem2 = Conteudo.Substring(separar + 1);                                  
                        if (!File.Exists(Conteudo))                                                             
                        {                                                                                            
                            throw new ExceptionFileNotFound("Ficheiro não encontrado!" , Conteudo);             
                        }                                                                                            
                        break;
                    case TipoDados.CaminhoDestino:
                        //Prepara os dados para fornecer ao Modelo                  
                        separar = Conteudo.LastIndexOf("/", StringComparison.Ordinal);               
                        modelo.PathDestino = Conteudo.Substring(0, separar+1);
                        modelo.FileDestino = Conteudo.Substring(separar + 1); 
                        break;
                    case TipoDados.CaminhoDestino2:
                        //Prepara os dados para fornecer ao Modelo                     
                        separar = Conteudo.LastIndexOf("/", StringComparison.Ordinal);                 
                        modelo.PathDestino2 = Conteudo.Substring(0, separar+1); 
                        modelo.FileDestino2 = Conteudo.Substring(separar + 1);  
                        break;
                    case TipoDados.Pagina:
                        modelo.Page = Valor;
                        break;
                    case TipoDados.PosicaoAdicionar:
                        modelo.AddPosition = Valor;
                        break;
                    case TipoDados.Senha:
                        modelo.Senha = Conteudo;
                        break;
                    case TipoDados.Texto:
                    case TipoDados.MarcaAgua:        
                        modelo.Texto = Conteudo; 
                        break;                   
                }
            }
            catch (ExceptionFileNotFound erro)
            {
                throw new ExceptionFileNotFound(erro);
            }
        }


        //Função que executa a funcionalidade
        public void ProcessarDados(OpcoesExecucao op)
        {
            try
            {
                switch (op)
                {
                    case OpcoesExecucao.ContarPaginas:
                        modelo.EfetuarProcesso = CountMod.ContarPaginas;
                        break;
                    case OpcoesExecucao.RemoverPagina:
                        modelo.EfetuarProcesso = EditMod.RemovePage;
                        break;
                    case OpcoesExecucao.AdicionarMarca:
                        modelo.EfetuarProcesso = EditMod.WatermarkFile;
                        break;
                    case OpcoesExecucao.AdicionarPagina:
                        modelo.EfetuarProcesso = EditMod.AddPage;
                        break;
                }
                modelo.EfetuarProcesso(modelo);
            }
            catch (ExceptionDadosInvalidos erro)
            {
                throw new ExceptionDadosInvalidos(erro);
            }
            catch (ExceptionFileNotFound erro)
            {
                throw new ExceptionFileNotFound(erro);
            }catch (DllNotFoundException erro)
            {
                throw new DllNotFoundException(erro.Message);
            }
        }
        public bool Executar
        {
            get => executar;
            set => executar = value;
        }

        public int Valor
        {
            get => valor;
            set => valor = value;
        }

        public string Conteudo
        {
            get => conteudo;
            set => conteudo = value;
        }
    }
}