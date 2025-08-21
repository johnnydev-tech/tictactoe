# Jogo da Velha - Godot C#

Um jogo da velha simples criado em Godot 4 usando C#.

## Características

- Interface responsiva que se adapta ao tamanho da tela
- Tabuleiro fixo do lado direito
- Controles simplificados via mouse e teclado
- Sistema de pontuação que mantém o score de vitórias
- Destaque visual das células vencedoras
- Navegação com Tab e seleção com Enter
- Cores diferentes para X (vermelho) e O (azul)

## Controles

### Mouse:
- **Clique**: Selecionar célula diretamente

### Teclado:
- **Tab**: Navegar entre as células
- **Enter**: Confirmar seleção da célula focada
- **R**: Reiniciar o jogo

## Layout da Tela

- **Painel Esquerdo (50%)**: 
  - Status do jogo (vez do jogador, placar)
  - Instruções de controle
  - Botão de reiniciar
- **Painel Direito (50%)**: 
  - Tabuleiro responsivo e ajustável
  - Se adapta automaticamente ao tamanho da tela

## Como Jogar

1. O jogo começa com o jogador X
2. Clique em uma célula ou use Tab + Enter para fazer sua jogada
3. Os jogadores se alternam automaticamente entre X e O
4. O primeiro jogador a conseguir três símbolos em linha vence
5. Se todas as células forem preenchidas sem vencedor, é empate
6. Use a tecla R ou o botão para reiniciar
7. O placar é mantido entre as partidas

## Responsividade

O tabuleiro se ajusta automaticamente ao tamanho da janela, mantendo sempre a proporção quadrada e ocupando o máximo de espaço disponível no painel direito.

## Estrutura do Projeto

- `Main.tscn`: Cena principal com a interface do jogo
- `GameManager.cs`: Script principal que gerencia a lógica do jogo
- `Cell.cs`: Script para cada célula do tabuleiro
- `project.godot`: Configurações do projeto Godot

## Requisitos

- Godot 4.4 ou superior
- Suporte a C# configurado no Godot

## Como Executar

1. Abra o projeto no Godot
2. Certifique-se de que o suporte a C# está habilitado
3. Execute o projeto (F5) ou clique no botão Play
