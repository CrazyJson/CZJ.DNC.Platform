using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CZJ.DNC.Web
{
    ///<summary>
    ///字典表
    ///</summary>
    [Table("common_dictionary")]
    public class SysDictionary
    {
        ///<summary>
        ///字典Id 
        ///</summary> 
        [Key, Column("DictionaryId"), Required]
        public string DictionaryId { get; set; }

        ///<summary>
        ///字典类型 
        ///</summary> 
        [Column("Kind"), Required]
        public int Kind { get; set; }

        ///<summary>
        ///字典编号 
        ///</summary> 
        [Column("DictionaryNo"), Required]
        public string DictionaryNo { get; set; }

        ///<summary>
        ///字典值 
        ///</summary> 
        [Column("DictionaryValue"), Required]
        public string DictionaryValue { get; set; }

        ///<summary>
        ///父级id 
        ///</summary> 
        [Column("ParentId")]
        public string ParentId { get; set; }

        ///<summary>
        ///当前层级排序值
        ///</summary> 
        [Column("SortNo")]
        public int? SortNo { get; set; }

        ///<summary>
        ///显示顺序 用于树形和列表排序用
        ///</summary> 
        [Column("ShowOrder")]
        public string ShowOrder { get; set; }

        ///<summary>
        ///层级代码 树形结构时 用于搜索 
        ///</summary> 
        [Column("HierarchyCode")]
        public string HierarchyCode { get; set; }

        ///<summary>
        ///备注 
        ///</summary> 
        [Column("Remark")]
        public string Remark { get; set; }

        ///<summary>
        ///是否系统级数据 默认为 0否 
        ///</summary> 
        [Column("IsSystem")]
        public bool? IsSystem { get; set; }

        ///<summary>
        ///拼音 
        ///</summary> 
        [Column("Bopomofo")]
        public string Bopomofo { get; set; }
    }
}

