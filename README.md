# binaryTreeWithGraphviz

This a small c# project to generate algebraic expressions in binary trees using graphviz! ![Image][1]

# TODO

1. Fix binaryTree library tree generator
   - Turns out that in the `add()` function of the library cannot really make an expression because of the way it compares strings.

```
int comparison = String.Compare(node.name, tree.name);

if (comparison == 0)
    throw new Exception();
if (comparison < 0)
{
    add(node, ref tree.left);
}
else
{
    add(node, ref tree.right);
}
```

2. Fix the `InfToPost` and `InfToPref` classes to use a common hierarchy class instead of an unique for each one.

[1]: /headGraph.jpg
