namespace ContIn.Abp.Terminal.Domain.Shared.Enum
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OpTypeEnum
    {
        // 创建
        Create,

        // 更新
        Update,

        // 删除
        Delete,

        // 禁用
        Forbidden,

        // 移除禁用
        RemoveForbidden
    }
}
