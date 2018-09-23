using System;

public class BinaryTree<T>
{
    private T Value;

    private BinaryTree<T> leftChild;

    private BinaryTree<T> rightChild;

    public BinaryTree(T value, BinaryTree<T> leftChild = null, BinaryTree<T> rightChild = null)
    {
        this.Value = value;
        this.leftChild = leftChild;
        this.rightChild = rightChild;
    }

    public void PrintIndentedPreOrder(int indent = 0)
    {
        Console.WriteLine(new string(' ', indent) + this.Value);

        if (this.leftChild != null)
        {
            this.leftChild.PrintIndentedPreOrder(indent + 2);
        }

        if (this.rightChild != null)
        {
            this.rightChild.PrintIndentedPreOrder(indent + 2);
        }
    }

    public void EachInOrder(Action<T> action)
    {
        this.InOrder(this, action);
    }

    private void InOrder(BinaryTree<T> binaryTree, Action<T> action)
    {
        if (binaryTree == null)
        {
            return;
        }

        binaryTree.InOrder(binaryTree.leftChild, action);
        action(binaryTree.Value);
        binaryTree.InOrder(binaryTree.rightChild, action);
    }

    public void EachPostOrder(Action<T> action)
    {
        this.PostOrder(this, action);
    }

    private void PostOrder(BinaryTree<T> binaryTree, Action<T> action)
    {
        if (binaryTree == null)
        {
            return;
        }

        binaryTree.PostOrder(binaryTree.leftChild, action);
        binaryTree.PostOrder(binaryTree.rightChild, action);
        action(binaryTree.Value);
    }
}
