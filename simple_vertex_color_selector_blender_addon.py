bl_info = {
    "name": "Simple Vertex Color Selector V3",
    "author": "Shadow Tzu / Gemini",
    "version": (3, 0, 0),
    "blender": (3, 0, 0),
    "location": "View3D > Sidebar (N) > Vertex Color",
    "description": "Sélectionne les faces par couleur (Version Sans Icônes)",
    "category": "Mesh",
}

import bpy
import bmesh

# --- LOGIQUE DE SÉLECTION ---
class VCSEL_OT_SelectByColor_V3(bpy.types.Operator):
    bl_idname = "vcsel.select_by_color_v3"
    bl_label = "Sélectionner par couleur"
    bl_options = {"REGISTER", "UNDO"}

    def execute(self, context):
        obj = context.active_object
        if not obj or obj.type != 'MESH':
            self.report({'ERROR'}, "Veuillez sélectionner un objet Mesh.")
            return {'CANCELLED'}

        mesh = obj.data
        props = context.scene.vcsel_v3_props
        target_rgb = props.pick_color[:3]

        # On repasse en mode objet pour lire les données
        if obj.mode != 'OBJECT':
            bpy.ops.object.mode_set(mode='OBJECT')

        # Trouver l'attribut de couleur
        col_attr = None
        if hasattr(mesh, "color_attributes") and mesh.color_attributes.active_color:
            col_attr = mesh.color_attributes.active_color
        elif hasattr(mesh, "vertex_colors") and mesh.vertex_colors.active:
            col_attr = mesh.vertex_colors.active

        if not col_attr:
            self.report({'ERROR'}, "Pas de Vertex Color trouvée.")
            return {'CANCELLED'}

        attr_data = col_attr.data
        matching_faces = []

        # Comparaison
        for poly in mesh.polygons:
            match = True
            for loop_idx in poly.loop_indices:
                col = attr_data[loop_idx].color[:3]
                if any(abs(col[i] - target_rgb[i]) > 0.001 for i in range(3)):
                    match = False
                    break
            if match:
                matching_faces.append(poly.index)

        # Passage en Edit Mode / Faces
        bpy.ops.object.mode_set(mode='EDIT')
        bm = bmesh.from_edit_mesh(mesh)
        bm.faces.ensure_lookup_table()
        
        context.tool_settings.mesh_select_mode = (False, False, True)

        for face in bm.faces:
            face.select = (face.index in matching_faces)

        bm.select_flush_mode()
        bmesh.update_edit_mesh(mesh)

        self.report({'INFO'}, f"Faces sélectionnées : {len(matching_faces)}")
        return {'FINISHED'}

# --- INTERFACE ---
class VCSEL_V3_Properties(bpy.types.PropertyGroup):
    pick_color: bpy.props.FloatVectorProperty(
        name="Couleur", 
        subtype="COLOR", 
        size=4, 
        min=0.0, max=1.0, 
        default=(1.0, 1.0, 1.0, 1.0)
    )

class VCSEL_PT_V3Panel(bpy.types.Panel):
    bl_label = "Vertex Color Picker"
    bl_idname = "VCSEL_PT_v3_panel"
    bl_space_type = "VIEW_3D"
    bl_region_type = "UI"
    bl_category = "Vertex Color"

    def draw(self, context):
        layout = self.layout
        props = context.scene.vcsel_v3_props
        
        col = layout.column(align=True)
        col.label(text="Couleur cible :")
        
        row_color = col.row()
        row_color.scale_y = 2.0
        row_color.prop(props, "pick_color", text="") 
        
        layout.separator()
        
        row_btn = layout.row()
        row_btn.scale_y = 2.0
        row_btn.operator("vcsel.select_by_color_v3", text="SÉLECTIONNER LES FACES")

# --- ENREGISTREMENT ---
classes = (VCSEL_V3_Properties, VCSEL_OT_SelectByColor_V3, VCSEL_PT_V3Panel)

def register():
    for cls in classes:
        bpy.utils.register_class(cls)
    bpy.types.Scene.vcsel_v3_props = bpy.props.PointerProperty(type=VCSEL_V3_Properties)

def unregister():
    for cls in reversed(classes):
        bpy.utils.unregister_class(cls)
    if hasattr(bpy.types.Scene, "vcsel_v3_props"):
        del bpy.types.Scene.vcsel_v3_props

if __name__ == "__main__":
    register()
