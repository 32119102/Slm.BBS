【仓储最佳实践&约定】
--------------推荐-----------------------
1、推荐 在领域层中定义仓储接口；
2、推荐 为每个聚合根订舱仓储接口并创建对应的实现；
3、推荐 通常仓储接口继承自 IBasicRepository<TEntity, TKey> 或更低级别的接口, 如 IReadOnlyRepository<TEntity, TKey> (在需要的时候)
4、推荐 所有的仓储方法定义为 异步；
5、推荐 为仓储的每个方法添加 可选参数 cancellationToken
6、推荐 为仓储的每个异步方法创建一个 同步扩展 方法
7、推荐 为仓储中返回单个实体的方法添加一个可选参数 bool includeDetails = true (默认值为true)
8、推荐 为仓储中返回实体列表的方法添加一个可选参数 bool includeDetails = false (默认值为false)

------------不推荐--------------------------
1、不推荐 在应用代码中使用泛型仓储接口；
2、不推荐 在应用代码(领域, 应用... 层)中使用 IQueryable<TEntity> 特性；
3、不推荐 仓储接口继承 IRepository<TEntity, TKey> 接口. 因为它继承了 IQueryable 而仓储不应该将IQueryable暴漏给应用；
4、不推荐 为实体定义仓储接口，因为它们不是聚合根；
5、不推荐 创建复合类通过调用仓储单个方法返回组合实体；相反, 正确的使用 includeDetails 选项, 在需要时加载实体所有的详细信息.