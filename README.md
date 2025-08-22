# 🎮 Jogo da Velha - Design Minimalista

## 🎨 **Layout Responsivo e Clean**

### � **Painel Esquerdo (35% da tela) - Minimalista:**
- **Título**: "Jogo da Velha" (clean, sem excesso)
- **Status**: Indicador simples de vez do jogador
- **Placar**: Formato limpo `X: 0  |  O: 0`
- **Controles**: Apenas 2 botões essenciais
  - `Reiniciar` - Nova partida
  - `Zerar Placar` - Reset das pontuações

### 🎯 **Painel Direito (65% da tela) - Foco no Jogo:**
- **Tabuleiro 3x3** centralizado e maior
- **Botões responsivos** com bom tamanho
- **Visual limpo** sem distrações

## ✨ **Características do Design:**

### 🔸 **Minimalista:**
- ❌ Removidos emojis excessivos
- ❌ Removidas instruções desnecessárias
- ❌ Removidos separadores visuais
- ✅ Apenas elementos essenciais
- ✅ Espaçamento inteligente com spacers
- ✅ Foco na experiência de jogo

### 🔸 **Responsivo:**
- **35% / 65%** - Proporção otimizada
- **Spacers** para distribuição automática
- **Tamanho adaptável** conforme a tela

### 🔸 **Usual e Familiar:**
- Interface similar a apps modernos
- Controles intuitivos e diretos
- Visual clean sem poluição

## � **Experiência de Uso:**

```
┌─────────────┬─────────────────────────┐
│  CONTROLES  │       TABULEIRO         │
│   (35%)     │        (65%)            │
├─────────────┼─────────────────────────┤
│             │                         │
│ Jogo da     │                         │
│ Velha       │       [ ][ ][ ]         │
│             │       [ ][ ][ ]         │
│ Vez do X    │       [ ][ ][ ]         │
│             │                         │
│ X: 0 | O: 0 │    (Tabuleiro maior     │
│             │     e centralizado)     │
│ [Reiniciar] │                         │
│             │                         │
│[Zerar Placar]│                        │
│             │                         │
└─────────────┴─────────────────────────┘
```

## 🚀 **Melhorias Implementadas:**

✅ **Interface mais limpa e profissional**
✅ **Foco no que importa: jogar**
✅ **Proporções ideais (35/65)**
✅ **Spacers para layout flexível**
✅ **Apenas 2 botões essenciais**
✅ **Placar adaptativo (mostra empates só quando há)**
✅ **Visual moderno e minimalista**

Agora o jogo tem uma interface clean, usual e focada na experiência! �

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
