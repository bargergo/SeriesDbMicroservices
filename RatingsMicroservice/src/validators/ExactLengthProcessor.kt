package hu.bme.aut.ratings.validators

import com.papsign.ktor.openapigen.model.schema.SchemaModel

object ExactLengthProcessor : LengthConstraintProcessor<ExactLength>() {
    override fun process(model: SchemaModel<*>, annotation: ExactLength): SchemaModel<*> {
        // There is no string schema and couldn't create one, because SchemaModel is a sealed class
        return model
    }

    override fun getConstraint(annotation: ExactLength): LengthConstraint {
        val errorMessage = if (annotation.message.isNotEmpty()) annotation.message else null
        return LengthConstraint(min = annotation.value, max = annotation.value, errorMessage = errorMessage)
    }
}